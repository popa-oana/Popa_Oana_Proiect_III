using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proiect_Netficks.Models;
using Proiect_Netficks.ViewModels;
using System.Collections.Generic;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class TopController : Controller
    {
        public IActionResult Index()
        {
            // In a real application, this would fetch data from a repository
            // For now, we'll create sample data
            var viewModel = new TopViewModel
            {
                TopFilms = GetSampleTopFilms(),
                TopSeries = GetSampleTopSeries()
            };
            
            return View(viewModel);
        }
        
        private List<Film> GetSampleTopFilms()
        {
            return new List<Film>
            {
                new Film { Film_ID = 1, Titlu = "Inception", An_Lansare = 2010, Durata = 148, Descriere = "Un hoț care fură secrete corporative prin utilizarea tehnologiei de partajare a viselor primește sarcina inversă de a planta o idee în mintea unui CEO." },
                new Film { Film_ID = 2, Titlu = "The Shawshank Redemption", An_Lansare = 1994, Durata = 142, Descriere = "Două persoane încarcerate formează o legătură de-a lungul anilor, găsind consolare și eventual răscumpărare prin acte de bunătate comună." },
                new Film { Film_ID = 3, Titlu = "The Dark Knight", An_Lansare = 2008, Durata = 152, Descriere = "Când amenințarea cunoscută sub numele de Joker provoacă haos și distrugere asupra locuitorilor din Gotham, Batman trebuie să accepte unul dintre cele mai mari teste psihologice și fizice pentru a lupta împotriva nedreptății." },
                new Film { Film_ID = 4, Titlu = "The Godfather", An_Lansare = 1972, Durata = 175, Descriere = "Patriarhul în vârstă al unei dinastii a crimei organizate transferă controlul imperiului său către fiul său reluctant." },
                new Film { Film_ID = 5, Titlu = "Pulp Fiction", An_Lansare = 1994, Durata = 154, Descriere = "Viețile a doi asasini plătiți, un boxer, soția unui gangster și doi bandiți se intersectează într-o poveste despre violență și răscumpărare." }
            };
        }
        
        private List<Serial> GetSampleTopSeries()
        {
            return new List<Serial>
            {
                new Serial { Serial_ID = 1, Titlu = "Breaking Bad", An_Aparitie = 2008, Numar_Sezoane = 5, Descriere = "Un profesor de chimie diagnosticat cu cancer inoperabil de plămâni se îndreaptă spre fabricarea și vânzarea de metamfetamină pentru a-și asigura viitorul financiar al familiei." },
                new Serial { Serial_ID = 2, Titlu = "Game of Thrones", An_Aparitie = 2011, Numar_Sezoane = 8, Descriere = "Nouă familii nobile luptă pentru controlul asupra ținuturilor mitice din Westeros, în timp ce un inamic străvechi se întoarce după ce a fost adormit mii de ani." },
                new Serial { Serial_ID = 3, Titlu = "Stranger Things", An_Aparitie = 2016, Numar_Sezoane = 4, Descriere = "Când un băiat dispare, orașul său dezvăluie un mister care implică experimente secrete, forțe supranaturale înfricoșătoare și o fată ciudată." },
                new Serial { Serial_ID = 4, Titlu = "The Crown", An_Aparitie = 2016, Numar_Sezoane = 5, Descriere = "Urmărește rivalitățile politice și romanța din timpul domniei Reginei Elisabeta a II-a și evenimentele care au modelat a doua jumătate a secolului XX." },
                new Serial { Serial_ID = 5, Titlu = "The Witcher", An_Aparitie = 2019, Numar_Sezoane = 2, Descriere = "Geralt de Rivia, un vânător de monștri singuratic, luptă pentru a-și găsi locul într-o lume în care oamenii se dovedesc adesea mai răi decât bestiile." }
            };
        }
    }
}
