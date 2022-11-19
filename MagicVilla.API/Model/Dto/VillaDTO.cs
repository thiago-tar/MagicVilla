using System.ComponentModel.DataAnnotations;

namespace MagicVilla.API.Model.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int Ocurrency { get; set; }

        public int Sfto { get; set; }


        public Villa ConvertToVilla()
        {
            Villa model = new()
            {
                Id = Id,
                Name = Name,
                Ocurrency = Ocurrency,
                Sfto = Sfto
            };

            return model;
        }
    }
}
