
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Autos" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Comida" });
                _context.Categories.Add(new Category { Name = "Cosmeticos" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Erotica" });
                _context.Categories.Add(new Category { Name = "Ferreteria" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Jardin" });
                _context.Categories.Add(new Category { Name = "Juguetes" });
                _context.Categories.Add(new Category { Name = "Lenceria" });
                _context.Categories.Add(new Category { Name = "Licores" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Nutricion" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Tecnologia" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { 
                    Name = "Mexico",
                    States = new List<State>()
                    {
                        new State() {
                            Name = "Queretaro",
                            Cities = new List<City>() {
                                new City() { Name = "Amealco" },
                                new City() { Name = "Colon" },
                                new City() { Name = "Queretaro" },
                            }
                        },
                        new State
                        {
                            Name = "Aguascalientes",
                            Cities = new List<City>()
                            {
                                new City { Name = "Aguascalientes" },
                                new City { Name = "Asientos" },
                                new City { Name = "Calvillo" },
                                new City { Name = "Cosio" },
                                new City { Name = "Jesus Maria" },
                                new City { Name = "Pabellon de Arteaga" },
                                new City { Name = "Rincon de Romos" },
                                new City { Name = "San Jose de Garcia" },
                                new City { Name = "Tepezala" },
                                new City { Name = "El Llano" },
                                new City { Name = "San Francisco de los Romo" },
                            }
                        },
                        new State
                        {
                            Name = "Baja California",
                            Cities = new List<City>()
                            {
                                new City { Name = "Ensenada" },
                                new City { Name = "Mexicali" },
                                new City { Name = "Tecate" },
                                new City { Name = "Tijuana" },
                                new City { Name = "Playas de Rosarito" },
                            },
                        },
                        new State
                        {
                            Name = "Baja California Sur",
                            Cities = new List<City>()
                            {
                                new City { Name = "Comondu" },
                                new City { Name = "Mulege" },
                                new City { Name = "La Paz" },
                                new City { Name = "Loz Cabos" },
                                new City { Name = "Loreto" },
                            },
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                            Cities = new List<City>() {
                                new City() { Name = "Medellín" },
                                new City() { Name = "Itagüí" },
                                new City() { Name = "Envigado" },
                                new City() { Name = "Bello" },
                                new City() { Name = "Rionegro" },
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>() {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Champinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Useme" },
                                new City() { Name = "Bosa" },
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
