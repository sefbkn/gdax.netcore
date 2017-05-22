using Newtonsoft.Json.Linq;

namespace Boukenken.Gdax
{
    public class Product
    {
        public string id { get; set; }
        public string base_currency { get; set;  }
        public string quote_currency { get; set;  }
        public decimal base_min_size { get; set;  }
        public decimal base_max_size { get; set;  }
        public decimal quote_increment { get; set;  }
    }
}