using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_Netficks.Models;
using System.Collections.Generic;

namespace Proiect_Netficks.ViewModels
{
    public class SearchViewModel
    {
        public string? Title { get; set; }
        public int? Year { get; set; }
        public int? GenreId { get; set; }
        public SelectList? Genres { get; set; }
        public List<SearchResultViewModel> SearchResults { get; set; } = new List<SearchResultViewModel>();
        public List<SerialSearchResultViewModel> SerialResults { get; set; } = new List<SerialSearchResultViewModel>();
    }

    public class SearchResultViewModel
    {
        public Film? Film { get; set; }
        public bool IsInWatchlist { get; set; }
    }
    
    public class SerialSearchResultViewModel
    {
        public Serial? Serial { get; set; }
        public bool IsInWatchlist { get; set; }
    }
}
