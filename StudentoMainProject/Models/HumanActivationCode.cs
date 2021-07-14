using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public enum CodeType { Teacher, Student }
    public class HumanActivationCode
    {
        public int Id { get; set; }
        public int TargetId { get; set; } //Teacher or student Id
        public string HumanCode { get; set; }
        [Display(Name = "Typ kódu")]
        public CodeType CodeType { get; set; }
        public string getNewHumanCode()
        {
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string code = "";
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                int indexOfChar = (int)Math.Floor(r.NextDouble() * 34);
                code += alphabet[indexOfChar];
            }
            return code;

        }
    }

}
