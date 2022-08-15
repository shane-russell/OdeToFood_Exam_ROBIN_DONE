using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OdeToFood.Data.DomainClasses
{
    public class ReviewModel
    {
        [Range(1, 10)]
        public int Rating { get; set; }

        public string Body { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        [Required]
        public string ReviewerName { get; set; }
    }
}
