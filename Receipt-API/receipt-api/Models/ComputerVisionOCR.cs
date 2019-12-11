namespace receipt_api
{
    public class OCRVisionResponse
    {
        public Region recognitionResult { get; set; } 
    }

    public class Region
    {
        public Line[] lines { get; set; }
    }

    public class Line
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
        public Word[] words { get; set; }
    }

    public class Word
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
    }
}
