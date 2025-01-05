using Domain.Interfaces;

namespace Domain.Models
{
    public class Image : IModel<int>
    {
        public int Id { get; }
        public string ImagePath { get; } = String.Empty;


        private Image() { }

        private Image(int id, string imagePath) 
        {
            Id = id;
            ImagePath = imagePath;
        }

        private Image(string imagePath) 
        {
            ImagePath = imagePath;
        }


        public static Image Create(string imagePath) 
        {
            return new Image(imagePath);
        }
    }
}
