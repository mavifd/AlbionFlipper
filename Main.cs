using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

/* 
 
ONE HAND | 288
TWO HAND | 384
OFF HAND | 96

HEAD | 96 
ARMOR | 192
SHOES | 96

CAPE | 96
BAG | 192

x.1   x.2   x.3
rune  soul  relic
 
*/

namespace AlbionFlipperServer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public class ItemData
        {
            public string Name { get; set; }
            public string Tier { get; set; }
            public string Level { get; set; }
            public string Code { get; set; }
            public bool OneHand { get; set; }
            public bool TwoHand { get; set; }
            public bool OffHand { get; set; }
            public bool Head { get; set; }
            public bool Armor { get; set; }
            public bool Shoes { get; set; }
            public bool Cape { get; set; }
            public bool Bag { get; set; }
        }

        public class LocalData
        {
            public long Id { get; set; }
            public string ItemTypeId { get; set; }
            public string ItemGroupTypeId { get; set; }
            public int LocationId { get; set; }
            public int QualityLevel { get; set; }
            public int EnchantmentLevel { get; set; }
            public long UnitPriceSilver { get; set; }
            public int Amount { get; set; }
            public string AuctionType { get; set; }
            public DateTime Expires { get; set; }
        }

        public class Root
        {
            public List<LocalData> Orders { get; set; }
        }

        private static ConcurrentQueue<string> PrimalData = new ConcurrentQueue<string>();
        private ConcurrentDictionary<long, LocalData> BlackMarketData = new ConcurrentDictionary<long, LocalData>();
        private ConcurrentDictionary<long, LocalData> CaerleonData = new ConcurrentDictionary<long, LocalData>();
        private List<ItemData> TierList = new List<ItemData>();

        public async Task InitItemList()
        {
            TierList.Clear();
            string url = "https://raw.githubusercontent.com/ao-data/ao-bin-dumps/master/formatted/items.txt";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    var lines = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string[] parts = line.Split(new[] { ':' }, 3);

                        bool onehand = false, twohand = false, offhand = false, head = false, armor = false, shoes = false, bag = false, cape = false;

                        if (parts.Length >= 3)
                        {
                            string tier = "";
                            string level = "";

                            if (line.Contains("_MAIN_")) onehand = true;
                            else if (line.Contains("_2H_")) twohand = true;

                            if (line.Contains("_OFF_")) offhand = true;

                            if (line.Contains("_HEAD_")) head = true;
                            if (line.Contains("_ARMOR_")) armor = true;
                            if (line.Contains("_SHOES_")) shoes = true;

                            if (line.Contains("_CAPEITEM_")) cape = true;
                            if (line.Contains("_BAG_")) bag = true;

                            if (parts[1].Trim().StartsWith("T1_")) tier = "T1";
                            else if (parts[1].Trim().StartsWith("T2_")) tier = "T2";
                            else if (parts[1].Trim().StartsWith("T3_")) tier = "T3";
                            else if (parts[1].Trim().StartsWith("T4_")) tier = "T4";
                            else if (parts[1].Trim().StartsWith("T5_")) tier = "T5";
                            else if (parts[1].Trim().StartsWith("T6_")) tier = "T6";
                            else if (parts[1].Trim().StartsWith("T7_")) tier = "T7";
                            else if (parts[1].Trim().StartsWith("T8_")) tier = "T8";

                            if (parts[1].Trim().EndsWith("@1")) level = "1";
                            else if (parts[1].Trim().EndsWith("@2")) level = "2";
                            else if (parts[1].Trim().EndsWith("@3")) level = "3";
                            else if (parts[1].Trim().EndsWith("@4")) level = "4";
                            else level = "0";


                            if (!string.IsNullOrEmpty(tier))
                            {
                                TierList.Add(new ItemData
                                {
                                    Name = parts[2].Trim(),
                                    Tier = tier,
                                    Level = level,
                                    Code = parts[1].Trim(),
                                    OneHand = onehand,
                                    TwoHand = twohand,
                                    OffHand = offhand,
                                    Head = head,
                                    Armor = armor,
                                    Shoes = shoes,
                                    Bag = bag,
                                    Cape = cape
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogApp($"Error fetching item list: {ex.Message}");
                }
            }
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            await InitItemList();
            _ = Task.Run(DataReceiverLoop);
            _ = Task.Run(DataManager);
        }

        private async Task DataReceiverLoop()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            LogApp("Listening...");
            while (true)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    string data = await reader.ReadToEndAsync();
                    PrimalData.Enqueue(data);
                    LogApp("Data received and enqueued.");
                }
            }
        }

        private async Task DataManager()
        {
            while (true)
            {
                while (PrimalData.TryDequeue(out var data))
                {
                    try
                    {
                        var localData = JsonConvert.DeserializeObject<Root>(data);
                        LogApp($"Processing {localData.Orders.Count}x data...");
                        foreach (var order in localData.Orders)
                        {
                            order.UnitPriceSilver /= 10000;
                            order.Expires = DateTime.Now;
                            if (order.LocationId == 3003)
                            {
                                BlackMarketData.AddOrUpdate(order.Id, order, (key, existingOrder) => order);
                            }
                            else if (order.LocationId == 3005)
                            {
                                CaerleonData.AddOrUpdate(order.Id, order, (key, existingOrder) => order);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogApp($"Error processing data: {ex.Message}");
                    }
                }
                await Task.Delay(100);
            }
        }

        private void ProfitChecker()
        {
            LogApp("Checking profits...");
            var maxProfitItems = new ConcurrentDictionary<string, (LocalData caeData, LocalData bmData, long profit)>();
            var minprofitVal = minprofit.Value;
            var bmtime = (double)bm_Numeric.Value;
            var caetime = (double)caerleon_Numeric.Value;

            foreach (var cae in CaerleonData) if ((DateTime.Now - cae.Value.Expires).Minutes > caetime) CaerleonData.TryRemove(cae.Key, out _);
            foreach (var bm in BlackMarketData) if ((DateTime.Now - bm.Value.Expires).Minutes > bmtime) BlackMarketData.TryRemove(bm.Key, out _);

            var caeList = CaerleonData.Values.Where(cae => (DateTime.Now - cae.Expires).Minutes <= caetime).ToList();
            var bmList = BlackMarketData.Values.Where(bm => (DateTime.Now - bm.Expires).Minutes <= bmtime).ToList();

            Parallel.ForEach(caeList, caeData =>
            {
                foreach (var bmData in bmList)
                {
                    if (bmData.ItemTypeId == caeData.ItemTypeId && bmData.EnchantmentLevel == caeData.EnchantmentLevel && caeData.QualityLevel >= bmData.QualityLevel)
                    {
                        var profit = bmData.UnitPriceSilver - caeData.UnitPriceSilver;
                        if (profit >= minprofitVal)
                        {
                            maxProfitItems.AddOrUpdate(caeData.ItemTypeId,
                                (caeData, bmData, profit),
                                (key, existing) => existing.profit < profit ? (caeData, bmData, profit) : existing);
                        }
                    }
                }
            });
            foreach (var item in maxProfitItems.Values)
            {
                var itemData = TierList.FirstOrDefault(i => i.Code == item.caeData.ItemTypeId);
                string itemName = $"{itemData?.Name} [{itemData?.Tier.Substring(1, 1)}.{itemData?.Level}]";
                TimeSpan timeDiff_Cae = DateTime.Now - item.caeData.Expires;
                TimeSpan timeDiff_BM = DateTime.Now - item.bmData.Expires;
                string caerleonPrice = $"x{item.caeData.Amount} {item.caeData.UnitPriceSilver:N0} - {GetQualityLevel(item.caeData.QualityLevel)} | {timeDiff_Cae.Minutes} dakika";
                string blackMarketPrice = $"x{item.bmData.Amount} {item.bmData.UnitPriceSilver:N0} - {GetQualityLevel(item.bmData.QualityLevel)} | {timeDiff_BM.Minutes} dakika";
                this.Invoke((Action)(() => { profitData.Rows.Add(itemName, (decimal)item.profit, caerleonPrice, blackMarketPrice); }));
            }
            LogApp("Checking profits done!");
        }

        private string GetQualityLevel(int quality)
        {
            var qualityLevels = new Dictionary<int, string> { { 1, "Normal" }, { 2, "Good" }, { 3, "Outstanding" }, { 4, "Excellent" }, { 5, "Masterpiece" } };
            return qualityLevels.ContainsKey(quality) ? qualityLevels[quality] : "Unknown";
        }

        private void LogApp(string message)
        {
            string text = $"[{DateTime.Now:dd/MM HH:mm:ss}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired) LoggerTXT.Invoke(new Action(() => { LoggerTXT.AppendText(text); LoggerTXT.SelectionStart = LoggerTXT.Text.Length; LoggerTXT.ScrollToCaret(); }));
            else { LoggerTXT.AppendText(text); LoggerTXT.SelectionStart = LoggerTXT.Text.Length; LoggerTXT.ScrollToCaret(); }
        }

        private async void CheckProfitsBtn_Click(object sender, EventArgs e)
        {
            profitData.Rows.Clear();
            await Task.Run(() => ProfitChecker());
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            profitData.Rows.Clear();
            BlackMarketData.Clear();
            CaerleonData.Clear();
        }
    }
}