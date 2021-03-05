using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Grade
    {
        public enum USERTYPE { Teacher, Student }

        public int Id { get; set; }
        [Display(Name = "Název")]
        [Required(ErrorMessage = "Zadejte prosím název")]
        public string Name { get; set; }
        [Display(Name = "Známka")]
        [Range(1, 5, ErrorMessage = "Zadejte známku mezi 1 a 5")]
        public int Value { get; private set; } //For table in the db
        public int StudentId { get; set; }
        public int SubjectInstanceId { get; set; }
        public USERTYPE AddedBy { get; set; }
        public int? GradeGroupId { get; set; }
        [Display(Name = "Datum přidání")]
        public DateTime Added { get; set; }

        /// <summary>
        /// Sets the grade value from normal input
        /// </summary>
        /// <param name="value">Grade value - please use values from 1 to 5, they can all have modifiers (+ or -) after the digit</param>
        /// <exception cref="ArgumentException">If the value is not found in our mapping</exception>
        public void SetGradeValue(string value)
        {
            this.Value = MapDisplayValueToInnerValue(value); //Letting the potential argument exception bubble up
        }
        /// <summary>
        /// Sets the grade value
        /// </summary>
        /// <param name="value">Grade value - please use values from -10 to 110</param>
        public void SetGradeValue(int value)
        {
            if (value <= 110 && value >= -10)
            {
                this.Value = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Grade value");
            }

        }
        /// <summary>
        /// Gets the decimal representation of grade value
        /// </summary>
        /// <returns>Float value examples: 1.48, 0.6, 5.0</returns>
        public float GetGradeValueInDecimal()
        {

            return MapInnerValueToDecimalValue(Value);
        }
        /// <summary>
        /// Gets the display grade value like 1+,5,3-; if value out of mapping then it returns the decimal
        /// </summary>
        /// <returns></returns>
        public string GetGradeValue()
        {
            return MapInnerValueToDisplayValue(Value);
        }
        /// <summary>
        /// Gets the internal grade value like 110, 57 or -10
        /// </summary>
        /// <returns></returns>
        public int GetInternalGradeValue() => Value;
        /// <summary>
        /// Maps the internal grade value (eg. 110) to the display value (eg. 1+)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MapInnerValueToDisplayValue(int value)
        {
            return value switch
            {
                110 => "1+",
                100 => "1",
                90 => "1-",
                85 => "2+",
                75 => "2",
                65 => "2-",
                60 => "3+",
                50 => "3",
                40 => "3-",
                35 => "4+",
                25 => "4",
                15 => "4-",
                10 => "5+",
                0 => "5",
                -10 => "5-",
                _ => (5 - (value / 25f)).ToString("f2")
            };
        }
        /// <summary>
        /// Maps the inner value (110) to the decimal value (0.6), lowest returned is 5.0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float MapInnerValueToDecimalValue(int value)
        {
            if (value <= 110 && value >= 0)
            {
                return (float)Math.Round(5 - (value / 25f), 2);
            }
            if (value > 110)
            {
                return 1.0f;
            }
            //Value is lower than -10
            return 5.0f;

        }
        /// <summary>
        /// Returns the mapped grade value
        /// </summary>
        /// <param name="value">Grade value - please use values from 1 to 5, they can all have modifiers (+ or -) after the digit</param>
        /// <exception cref="ArgumentException">If the value is not found in our mapping</exception>
        public static int MapDisplayValueToInnerValue(string value)
        {
            return value switch
            {
                "1*" => 110,
                "1+" => 110,
                "1" => 100,
                "1-" => 90,
                "2+" => 85,
                "2" => 75,
                "2-" => 65,
                "3+" => 60,
                "3" => 50,
                "3-" => 40,
                "4+" => 35,
                "4" => 25,
                "4-" => 15,
                "5+" => 10,
                "5" => 0,
                "5-" => -10,
                _ => throw new ArgumentException("Value provided was not found in the mapping table", "Grade value"),
            };
        }


        public GradeGroup GradeGroup { get; set; }
        public Student Student { get; set; }
        public SubjectInstance SubjectInstance { get; set; }
    }

}
