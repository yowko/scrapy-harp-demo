using System.Web;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

// 建立瀏覽器物件
ScrapingBrowser mybrowser = new ScrapingBrowser(); 

// 以 yahoo 股票並選擇 ETF 為例
WebPage webpage = mybrowser.NavigateToPage(new Uri("https://tw.stock.yahoo.com/class-quote?sectorId=26&exchange=TAI")); 
var html = webpage.Html;

//使用 css selector 取得所有 etf 股票的 html node
var nodes = html.CssSelect("li[class='List(n)']");

//將 html node 轉換成 Stock 物件
var stocks=nodes.Select(a => new Stock
{
    //以下使用 xpath 取得股票資訊
    Name = HttpUtility.HtmlDecode(a.SelectSingleNode("./div/div[1]/div[2]/div/div[1]").InnerText.Trim()),//將 html entity 轉換成字串
    Symbol = a.SelectSingleNode("./div/div[1]/div[2]/div/div[2]").InnerText.Trim(),
    Price =  a.SelectSingleNode("./div/div[2]").InnerText.Trim(),
    PriceChange = a.SelectSingleNode("./div/div[3]").InnerText.Trim(),
    Change= a.SelectSingleNode("./div/div[4]").InnerText.Trim(),
    Open = a.SelectSingleNode("./div/div[5]").InnerText.Trim(),
    LastClose = a.SelectSingleNode("./div/div[6]").InnerText.Trim(),
    High = a.SelectSingleNode("./div/div[7]").InnerText.Trim(),
    Low = a.SelectSingleNode("./div/div[8]").InnerText.Trim(),
    Turnover = a.SelectSingleNode("./div/div[9]").InnerText.Trim(),
    UpDown = UpDownCheck(a.SelectSingleNode("./div/div[3]/span").Attributes["class"].Value) //處理上漲或下跌的顯示
});

foreach(var stock in stocks) 
{
    Console.WriteLine($"股票名稱: {stock.Name.PadRight(12)}\t 股票代號: {stock.Symbol}\t 股價: {stock.Price.PadRight(5)}\t 漲跌: {stock.UpDown} {stock.PriceChange.PadRight(8)}\t 漲跌幅: {stock.UpDown} {stock.Change.PadRight(8)}\t 開盤: {stock.Open}\t 昨收: {stock.LastClose}\t 最高: {stock.High}\t 最低: {stock.Low}\t 成交量(張): {stock.Turnover}");
} 

string UpDownCheck(string value)
{
    if (value.Contains("up"))
    {
        return "上漲";
    }
    if (value.Contains("down"))
    {
        return "下跌";
    }
    return string.Empty;
}

class Stock
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Price { get; set; }
    public string Change { get; set; }
    public string PriceChange { get; set; }
    public string Open { get; set; }
    public string LastClose { get; set; }
    public string High { get; set; }
    public string Low { get; set; }
    public string Turnover { get; set; }
    public string UpDown { get; set; }
} 