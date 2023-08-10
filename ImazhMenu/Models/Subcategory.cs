namespace ImazhMenu.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string SubCactegoryName { get; set; }
        public string Description { get; set; }
        public string SubCatImgUrl { get; set; }
        public int Price { get; set; }
        public int CategoryRef { get; set; }
        public Category Category { get; set; }
    }
}
