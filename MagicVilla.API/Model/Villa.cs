using MagicVilla.API.Model.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.API.Model
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Ocurrency { get; set; }

        public int Sfto { get; set; }

        public DateTime CreateDate { get; set; }

        public VillaDTO ConvertToVillaDTO()
        {
            VillaDTO model = new()
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
