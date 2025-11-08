using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proiect_Netficks.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data
{
    public static class DBInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create admin role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Create admin user if it doesn't exist
                var adminEmail = "admin@netficks.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Nume = "Administrator",
                        Tip_Abonament = "Premium",
                        Data_Inregistrare = DateTime.Now,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // Look for any films
                if (context.Filme.Any())
                {
                    return;   // DB has been seeded
                }

                // Add genres first
                var genuri = new Gen[]
                {
                    new Gen { Nume_Gen = "Acțiune" },
                    new Gen { Nume_Gen = "Comedie" },
                    new Gen { Nume_Gen = "Dramă" },
                    new Gen { Nume_Gen = "Științifico-fantastic" },
                    new Gen { Nume_Gen = "Horror" },
                    new Gen { Nume_Gen = "Thriller" },
                    new Gen { Nume_Gen = "Animație" },
                    new Gen { Nume_Gen = "Aventură" },
                    new Gen { Nume_Gen = "Documentar" },
                    new Gen { Nume_Gen = "Romantic" }
                };

                context.Genuri.AddRange(genuri);
                await context.SaveChangesAsync();

                // Add films
                var filme = new Film[]
                {
                    new Film
                    {
                        Gen_ID = genuri[3].Gen_ID, // Științifico-fantastic
                        Titlu = "Inception",
                        An_Lansare = 2010,
                        Durata = 148,
                        Descriere = "Un hoț specializat în extragerea de secrete din subconștientul uman primește sarcina de a planta o idee în mintea unei persoane."
                    },
                    new Film
                    {
                        Gen_ID = genuri[0].Gen_ID, // Acțiune
                        Titlu = "John Wick",
                        An_Lansare = 2014,
                        Durata = 101,
                        Descriere = "Un fost asasin iese din pensie pentru a se răzbuna pe gangsterii care i-au ucis câinele."
                    },
                    new Film
                    {
                        Gen_ID = genuri[1].Gen_ID, // Comedie
                        Titlu = "The Hangover",
                        An_Lansare = 2009,
                        Durata = 100,
                        Descriere = "Trei prieteni încearcă să-și amintească evenimentele dintr-o petrecere a burlacilor din Las Vegas pentru a-l găsi pe mirele dispărut."
                    },
                    new Film
                    {
                        Gen_ID = genuri[2].Gen_ID, // Dramă
                        Titlu = "The Shawshank Redemption",
                        An_Lansare = 1994,
                        Durata = 142,
                        Descriere = "Povestea unui bancher condamnat pe nedrept la închisoare pe viață și prietenia sa cu un alt deținut."
                    },
                    new Film
                    {
                        Gen_ID = genuri[4].Gen_ID, // Horror
                        Titlu = "The Conjuring",
                        An_Lansare = 2013,
                        Durata = 112,
                        Descriere = "Investigatori paranormali ajută o familie terorizată de o prezență întunecată în casa lor."
                    },
                    new Film
                    {
                        Gen_ID = genuri[3].Gen_ID, // Științifico-fantastic
                        Titlu = "Interstellar",
                        An_Lansare = 2014,
                        Durata = 169,
                        Descriere = "Un grup de astronauți călătoresc prin spațiu în căutarea unei noi planete pentru umanitate."
                    },
                    new Film
                    {
                        Gen_ID = genuri[6].Gen_ID, // Animație
                        Titlu = "Toy Story",
                        An_Lansare = 1995,
                        Durata = 81,
                        Descriere = "Jucăriile unui băiat prind viață când nimeni nu le vede."
                    },
                    new Film
                    {
                        Gen_ID = genuri[7].Gen_ID, // Aventură
                        Titlu = "Pirates of the Caribbean",
                        An_Lansare = 2003,
                        Durata = 143,
                        Descriere = "Un pirat excentric și un fierar se unesc pentru a salva o tânără răpită."
                    },
                    new Film
                    {
                        Gen_ID = genuri[8].Gen_ID, // Documentar
                        Titlu = "Planet Earth",
                        An_Lansare = 2006,
                        Durata = 550, // Total duration of series
                        Descriere = "Un documentar care prezintă diversitatea habitatelor naturale de pe Pământ."
                    },
                    new Film
                    {
                        Gen_ID = genuri[9].Gen_ID, // Romantic
                        Titlu = "The Notebook",
                        An_Lansare = 2004,
                        Durata = 123,
                        Descriere = "O poveste de dragoste răvășitoare între un tânăr muncitor și o fată bogată."
                    },
                    new Film
                    {
                        Gen_ID = genuri[5].Gen_ID, // Thriller
                        Titlu = "The Silence of the Lambs",
                        An_Lansare = 1991,
                        Durata = 118,
                        Descriere = "Un agent FBI trebuie să coopereze cu un criminal psihopat pentru a prinde un alt criminal în serie."
                    },
                    new Film
                    {
                        Gen_ID = genuri[0].Gen_ID, // Acțiune
                        Titlu = "The Dark Knight",
                        An_Lansare = 2008,
                        Durata = 152,
                        Descriere = "Batman trebuie să accepte una dintre cele mai mari provocări psihologice și fizice reprezentate de criminalul cunoscut sub numele de Joker."
                    }
                };

                context.Filme.AddRange(filme);
                await context.SaveChangesAsync();
            }
        }
    }
}
