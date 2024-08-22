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
using System.Text;

namespace AlbionFlipperServer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public class DataEntry
        {
            [JsonProperty("item_count")]
            public int ItemCount { get; set; }

            [JsonProperty("avg_price")]
            public int AvgPrice { get; set; }

            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }
        }

        public class UpgradeItemData
        {
            [JsonProperty("location")]
            public string Location { get; set; }

            [JsonProperty("item_id")]
            public string ItemId { get; set; }

            [JsonProperty("quality")]
            public int Quality { get; set; }

            [JsonProperty("data")]
            public List<DataEntry> Data { get; set; }
        }

        public class ItemData
        {
            public string Name { get; set; }
            public int Tier { get; set; }
            public int Enchantment { get; set; }
            public string Code { get; set; }
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

        private ConcurrentDictionary<long, LocalData> BlackMarketData = new ConcurrentDictionary<long, LocalData>();
        private ConcurrentDictionary<long, LocalData> CaerleonData = new ConcurrentDictionary<long, LocalData>();
        private List<ItemData> TierList = new List<ItemData>();

        int runet4 = 0;
        int runet5 = 0;
        int runet6 = 0;
        int runet7 = 0;
        int runet8 = 0;

        int soult4 = 0;
        int soult5 = 0;
        int soult6 = 0;
        int soult7 = 0;
        int soult8 = 0;

        int relict4 = 0;
        int relict5 = 0;
        int relict6 = 0;
        int relict7 = 0;
        int relict8 = 0;

        int onehandupcost = 288;
        int twohandupcost = 384;
        int offhandupcost = 96;

        int headupcost = 96;
        int armorupcost = 192;
        int shoesupcost = 96;

        int capeupcost = 96;
        int bagupcost = 192;

        private async Task<string> UpdateCostRequest()
        {
            string items = "T4_RUNE,T5_RUNE,T6_RUNE,T7_RUNE,T8_RUNE,T4_SOUL,T5_SOUL,T6_SOUL,T7_SOUL,T8_SOUL,T4_RELIC,T5_RELIC,T6_RELIC,T7_RELIC,T8_RELIC";
            string apiUrl = $"https://europe.albion-online-data.com/api/v2/stats/History/{items}?locations=Caerleon&qualities=1&time-scale=1";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    Stream decompressedStream = response.Content.Headers.ContentEncoding.Contains("gzip")
                        ? new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress)
                        : responseStream;
                    using (StreamReader reader = new StreamReader(decompressedStream))
                    {
                        string responseBody = await reader.ReadToEndAsync();
                        return responseBody;
                    }
                }
                catch (Exception e)
                {
                    LogApp($"İstek sırasında hata oluştu: {e.Message}");
                }
            }
            return null;
        }

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

                        if (parts.Length >= 3)
                        {
                            int tier = 0;
                            int level = 0;

                            if (parts[1].Trim().StartsWith("T1_")) tier = 1;
                            else if (parts[1].Trim().StartsWith("T2_")) tier = 2;
                            else if (parts[1].Trim().StartsWith("T3_")) tier = 3;
                            else if (parts[1].Trim().StartsWith("T4_")) tier = 4;
                            else if (parts[1].Trim().StartsWith("T5_")) tier = 5;
                            else if (parts[1].Trim().StartsWith("T6_")) tier = 6;
                            else if (parts[1].Trim().StartsWith("T7_")) tier = 7;
                            else if (parts[1].Trim().StartsWith("T8_")) tier = 8;

                            if (parts[1].Trim().EndsWith("@1")) level = 1;
                            else if (parts[1].Trim().EndsWith("@2")) level = 2;
                            else if (parts[1].Trim().EndsWith("@3")) level = 3;
                            else if (parts[1].Trim().EndsWith("@4")) level = 4;
                            else level = 0;

                            TierList.Add(new ItemData
                            {
                                Name = parts[2].Trim(),
                                Tier = tier,
                                Enchantment = level,
                                Code = parts[1].Trim()
                            });

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
            await GetNewUpgradePrices();
            await Task.Run(() => DataReceiverLoop());
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
                _ = Task.Run(() => ProcessRequest(context));
            }
        }
        private async Task ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string data = await reader.ReadToEndAsync();
                var localData = JsonConvert.DeserializeObject<Root>(data);
                LogApp($"Processing {localData.Orders.Count}x data...");
                foreach (var order in localData.Orders)
                {
                    order.UnitPriceSilver /= 10000;
                    order.Expires = DateTime.Now;
                    if (order.LocationId == 3003) BlackMarketData.AddOrUpdate(order.Id, order, (key, existingOrder) => order);
                    else if (order.LocationId == 3005) CaerleonData.AddOrUpdate(order.Id, order, (key, existingOrder) => order);

                }
            }
        }

        private async Task GetNewUpgradePrices()
        {

            if (upgradecloudCh.Checked)
            {
                var itemIds = new[] {
        "T4_RUNE", "T5_RUNE", "T6_RUNE", "T7_RUNE", "T8_RUNE",
        "T4_SOUL", "T5_SOUL", "T6_SOUL", "T7_SOUL", "T8_SOUL",
        "T4_RELIC", "T5_RELIC", "T6_RELIC", "T7_RELIC", "T8_RELIC"};

                var Response = await UpdateCostRequest();
                var ParsedResp = JsonConvert.DeserializeObject<List<UpgradeItemData>>(Response);

                var latestPrices = new Dictionary<string, int>();

                foreach (var serverData in ParsedResp)
                {
                    if (itemIds.Contains(serverData.ItemId))
                    {
                        var latestDatum = serverData.Data.OrderByDescending(d => d.Timestamp).FirstOrDefault();
                        if (latestDatum != null)
                        {
                            LogApp($"{serverData.ItemId} - {latestDatum.AvgPrice} - {latestDatum.Timestamp.AddHours(3)}");
                            latestPrices[serverData.ItemId] = latestDatum.AvgPrice;
                        }
                    }
                }

                foreach (var kvp in latestPrices)
                {
                    if (kvp.Key == "T4_RUNE") runet4val.Value = kvp.Value;
                    else if (kvp.Key == "T5_RUNE") runet5val.Value = kvp.Value;
                    else if (kvp.Key == "T6_RUNE") runet6val.Value = kvp.Value;
                    else if (kvp.Key == "T7_RUNE") runet7val.Value = kvp.Value;
                    else if (kvp.Key == "T8_RUNE") runet8val.Value = kvp.Value;
                    else if (kvp.Key == "T4_SOUL") soult4val.Value = kvp.Value;
                    else if (kvp.Key == "T5_SOUL") soult5val.Value = kvp.Value;
                    else if (kvp.Key == "T6_SOUL") soult6val.Value = kvp.Value;
                    else if (kvp.Key == "T7_SOUL") soult7val.Value = kvp.Value;
                    else if (kvp.Key == "T8_SOUL") soult8val.Value = kvp.Value;
                    else if (kvp.Key == "T4_RELIC") relict4val.Value = kvp.Value;
                    else if (kvp.Key == "T5_RELIC") relict5val.Value = kvp.Value;
                    else if (kvp.Key == "T6_RELIC") relict6val.Value = kvp.Value;
                    else if (kvp.Key == "T7_RELIC") relict7val.Value = kvp.Value;
                    else if (kvp.Key == "T8_RELIC") relict8val.Value = kvp.Value;
                }
            }
            UpdatePrices();
        }


        private void UpdatePrices()
        {
            runet4 = (int)runet4val.Value;
            runet5 = (int)runet5val.Value;
            runet6 = (int)runet6val.Value;
            runet7 = (int)runet7val.Value;
            runet8 = (int)runet8val.Value;

            soult4 = (int)soult4val.Value;
            soult5 = (int)soult5val.Value;
            soult6 = (int)soult6val.Value;
            soult7 = (int)soult7val.Value;
            soult8 = (int)soult8val.Value;

            relict4 = (int)relict4val.Value;
            relict5 = (int)relict5val.Value;
            relict6 = (int)relict6val.Value;
            relict7 = (int)relict7val.Value;
            relict8 = (int)relict8val.Value;
        }
        private long CalcUpgradeCost(string itemTypeId, int currentLevel, int targetLevel, int tier)
        {
            UpdatePrices();

            long runeCost = 0, soulCost = 0, relicCost = 0, upgradeCost = 0; ;
            bool onehand = itemTypeId.Contains("_MAIN_");
            bool twohand = itemTypeId.Contains("_2H_");
            bool offhand = itemTypeId.Contains("_OFF_");
            bool head = itemTypeId.Contains("_HEAD_");
            bool armor = itemTypeId.Contains("_ARMOR_");
            bool shoes = itemTypeId.Contains("_SHOES_");
            bool cape = itemTypeId.Contains("_CAPEITEM_");
            bool bag = itemTypeId.Contains("_BAG_") || itemTypeId.Contains("_CAPE@") || itemTypeId == ($"{itemTypeId.Substring(0, 2)}_CAPE");

            switch (tier)
            {
                case 4: runeCost = runet4; soulCost = soult4; relicCost = relict4; break;
                case 5: runeCost = runet5; soulCost = soult5; relicCost = relict5; break;
                case 6: runeCost = runet6; soulCost = soult6; relicCost = relict6; break;
                case 7: runeCost = runet7; soulCost = soult7; relicCost = relict7; break;
                case 8: runeCost = runet8; soulCost = soult8; relicCost = relict8; break;
            }

            if (currentLevel < targetLevel)
            {
                int upgradeFromLevel = currentLevel + 1;

                for (int i = upgradeFromLevel; i <= targetLevel; i++)
                {
                    long cost = 0;
                    switch (i)
                    {
                        case 1: cost = (onehand ? runeCost * onehandupcost : twohand ? runeCost * twohandupcost : offhand ? runeCost * offhandupcost : head ? runeCost * headupcost : armor ? runeCost * armorupcost : shoes ? runeCost * shoesupcost : cape ? runeCost * capeupcost : bag ? runeCost * bagupcost : 0); break;
                        case 2: cost = (onehand ? soulCost * onehandupcost : twohand ? soulCost * twohandupcost : offhand ? soulCost * offhandupcost : head ? soulCost * headupcost : armor ? soulCost * armorupcost : shoes ? soulCost * shoesupcost : cape ? soulCost * capeupcost : bag ? soulCost * bagupcost : 0); break;
                        case 3: cost = (onehand ? relicCost * onehandupcost : twohand ? relicCost * twohandupcost : offhand ? relicCost * offhandupcost : head ? relicCost * headupcost : armor ? relicCost * armorupcost : shoes ? relicCost * shoesupcost : cape ? relicCost * capeupcost : bag ? relicCost * bagupcost : 0); break;
                    }
                    upgradeCost += cost;
                }
            }
            return upgradeCost;
        }

        private static readonly object syncObj = new object();

        private void ProfitChecker()
        {
            LogApp("Checking profits...");

            var maxProfitItems = new ConcurrentDictionary<string, (LocalData caeData, LocalData bmData, long profit)>();
            var maxProfitUpItems = new ConcurrentDictionary<string, (LocalData caeData, LocalData bmData, long profit, long cost)>();
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
                    var caeItemData = TierList.FirstOrDefault(i => i.Code == caeData.ItemTypeId);
                    var bmItemData = TierList.FirstOrDefault(i => i.Code == bmData.ItemTypeId);
                    if (caeItemData.Name == bmItemData.Name && caeItemData.Tier == bmItemData.Tier && caeData.QualityLevel >= bmData.QualityLevel)
                    {
                        if (bmItemData.Enchantment != 4 && caeItemData.Enchantment < 3)
                        {
                            long initialCost = caeData.UnitPriceSilver;
                            long totalCost = initialCost;

                            Parallel.For(caeData.EnchantmentLevel, bmData.EnchantmentLevel, currentLevel =>
                            {
                                int targetLevel = currentLevel + 1;
                                var upgradeCost = CalcUpgradeCost(caeData.ItemTypeId, currentLevel, targetLevel, caeItemData.Tier);

                                lock (syncObj)
                                {
                                    totalCost += upgradeCost;
                                }

                                if (bmItemData.Enchantment == targetLevel && bmData.UnitPriceSilver > totalCost)
                                {
                                    var profit = bmData.UnitPriceSilver - totalCost;
                                    if (profit >= minprofitVal)
                                    {
                                        lock (syncObj)
                                        {
                                            maxProfitUpItems.AddOrUpdate(caeData.ItemTypeId,
                                                (caeData, bmData, profit, totalCost),
                                                (key, existing) => existing.profit < profit ? (caeData, bmData, profit, totalCost) : existing);
                                        }
                                    }
                                }
                            });
                        }
                    }


                    if (caeData.ItemTypeId == bmData.ItemTypeId && caeData.EnchantmentLevel == bmData.EnchantmentLevel && caeData.QualityLevel >= bmData.QualityLevel)
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

            var rowsToAdd = new List<object[]>();
            foreach (var item in maxProfitItems.Values)
            {
                var itemData = TierList.FirstOrDefault(i => i.Code == item.caeData.ItemTypeId);
                string itemName = $"{itemData?.Name} [{itemData?.Tier}.{itemData?.Enchantment}]";
                TimeSpan timeDiff_Cae = DateTime.Now - item.caeData.Expires;
                TimeSpan timeDiff_BM = DateTime.Now - item.bmData.Expires;
                string caerleonPrice = $"x{item.caeData.Amount} {item.caeData.UnitPriceSilver:N0} - {GetQualityLevel(item.caeData.QualityLevel)} | {timeDiff_Cae.Minutes} dakika";
                string blackMarketPrice = $"x{item.bmData.Amount} {item.bmData.UnitPriceSilver:N0} - {GetQualityLevel(item.bmData.QualityLevel)} | {timeDiff_BM.Minutes} dakika";
                var caeItemData = TierList.FirstOrDefault(i => i.Code == item.caeData.ItemTypeId);
                var bmItemData = TierList.FirstOrDefault(i => i.Code == item.bmData.ItemTypeId);
                rowsToAdd.Add(new object[] { itemName, (decimal)item.profit, caerleonPrice, blackMarketPrice });
            }
            foreach (var item in maxProfitUpItems.Values)
            {
                var caeitemData = TierList.FirstOrDefault(i => i.Code == item.caeData.ItemTypeId);
                var bmitemData = TierList.FirstOrDefault(i => i.Code == item.bmData.ItemTypeId);
                var upcost = item.cost - item.caeData.UnitPriceSilver;

                int costcount = 0;
                if (item.caeData.ItemTypeId.Contains("_MAIN_")) costcount = onehandupcost;
                else if (item.caeData.ItemTypeId.Contains("_2H_")) costcount = twohandupcost;
                else if (item.caeData.ItemTypeId.Contains("_OFF_")) costcount = offhandupcost;
                else if (item.caeData.ItemTypeId.Contains("_HEAD_")) costcount = headupcost;
                else if (item.caeData.ItemTypeId.Contains("_ARMOR_")) costcount = armorupcost;
                else if (item.caeData.ItemTypeId.Contains("_SHOES_")) costcount = shoesupcost;
                else if (item.caeData.ItemTypeId.Contains("_CAPEITEM_") || item.caeData.ItemTypeId.Contains("_CAPE@") || item.caeData.ItemTypeId == ($"{item.caeData.ItemTypeId.Substring(0, 2)}_CAPE")) costcount = capeupcost;
                else if (item.caeData.ItemTypeId.Contains("_BAG_")) costcount = bagupcost;

                bool level1up = false;
                bool level2up = false;
                bool level3up = false;
                if (caeitemData.Enchantment == 0) // 6.0
                {
                    if (bmitemData.Enchantment == 1) { level1up = true; } // 6.1
                    if (bmitemData.Enchantment == 2) { level1up = true; level2up = true; } // 6.2
                    if (bmitemData.Enchantment == 3) { level1up = true; level2up = true; level3up = true; } // 6.3
                }
                else if (caeitemData.Enchantment == 1) // 6.1
                {
                    if (bmitemData.Enchantment == 2) { level2up = true; } // 6.2
                    if (bmitemData.Enchantment == 3) { level2up = true; level3up = true; } // 6.3
                }
                else if (caeitemData.Enchantment == 2) // 6.2
                {
                    if (bmitemData.Enchantment == 3) { level3up = true; } // 6.3
                }

                var sb = new StringBuilder();
                sb.Append($"T{caeitemData.Tier} {costcount}x ");
                if (level1up) sb.Append("Rune ");
                if (level2up) sb.Append("Soul ");
                if (level3up) sb.Append("Relic");
                string showupgradeinstruction = sb.ToString().Trim();

                string itemName = $"{caeitemData?.Name} [{caeitemData?.Tier}.{caeitemData?.Enchantment}]";
                TimeSpan timeDiff_Cae = DateTime.Now - item.caeData.Expires;
                TimeSpan timeDiff_BM = DateTime.Now - item.bmData.Expires;
                string caerleonPrice = $"x{item.caeData.Amount} {item.caeData.UnitPriceSilver:N0} - {GetQualityLevel(item.caeData.QualityLevel)} | {timeDiff_Cae.Minutes} dakika";
                string blackMarketPrice = $"x{item.bmData.Amount} {item.bmData.UnitPriceSilver:N0} - {GetQualityLevel(item.bmData.QualityLevel)} | {timeDiff_BM.Minutes} dakika";
                var caeItemData = TierList.FirstOrDefault(i => i.Code == item.caeData.ItemTypeId);
                var bmItemData = TierList.FirstOrDefault(i => i.Code == item.bmData.ItemTypeId);
                string upgraderow = $"[{caeitemData?.Tier}.{caeitemData?.Enchantment}] -> [{bmitemData?.Tier}.{bmitemData?.Enchantment}] Cost: {item.cost:N0} | {showupgradeinstruction}";

                rowsToAdd.Add(new object[] { itemName, (decimal)item.profit, caerleonPrice, blackMarketPrice, upgraderow });
            }
            this.Invoke((Action)(() => { foreach (var row in rowsToAdd) profitData.Rows.Add(row); }));

            LogApp("Checking profits done!");
        }

        private string GetQualityLevel(int quality)
        {
            var qualityLevels = new Dictionary<int, string> { { 1, "Normal" }, { 2, "Good" }, { 3, "Outstanding" }, { 4, "Excellent" }, { 5, "Masterpiece" } };
            return qualityLevels.ContainsKey(quality) ? qualityLevels[quality] : "Unknown";
        }


        private async void UpdateCosts_Click(object sender, EventArgs e)
        {
            await GetNewUpgradePrices();
        }

        private void ClearData_Click(object sender, EventArgs e)
        {
            profitData.Rows.Clear();
            BlackMarketData.Clear();
            CaerleonData.Clear();
        }

        private async void CheckProfits_Click(object sender, EventArgs e)
        {
            profitData.Rows.Clear();
            await Task.Run(() => ProfitChecker());
        }

        private void LogApp(string message)
        {
            string text = $"[{DateTime.Now:dd/MM HH:mm:ss}] {message}{Environment.NewLine}";
            if (LoggerTXT.InvokeRequired) LoggerTXT.Invoke(new Action(() => { LoggerTXT.AppendText(text); LoggerTXT.SelectionStart = LoggerTXT.Text.Length; LoggerTXT.ScrollToCaret(); }));
            else { LoggerTXT.AppendText(text); LoggerTXT.SelectionStart = LoggerTXT.Text.Length; LoggerTXT.ScrollToCaret(); }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = false;
            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            base.WndProc(ref msg);
        }


    }
}