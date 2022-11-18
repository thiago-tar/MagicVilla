using MagicVilla.API.Model.Dto;

namespace MagicVilla.API.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillaDTOs = new List<VillaDTO>()
            {
                new VillaDTO() { Id = 1, Name = "Konohagakure", Ocurrency = 2, Sfto = 100 },
                new VillaDTO() { Id = 2, Name = "Kirigakure", Ocurrency = 3, Sfto = 200 }

            };
    }
}
