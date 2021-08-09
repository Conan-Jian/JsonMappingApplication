using System;
using System.IO;
using System.Collections.Generic;

namespace JsonMappingApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Name of channel" +
                "\nExample: CommerceHub-JcPenny" +
                "\nChannel: ");
            string platform = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Enter Platform or Marketplace" +
                "\nPlaform or Marketplace? ");
            string type = Console.ReadLine();
            Console.WriteLine();
            GetJsonTemplateMapping(platform, type, RefineData());

        }
        public static List<string> RefineData()
        {
            //makes sure that there are no duplicates
            var result = new List<string>();

            using (StreamReader reader = new StreamReader("../../../Data.txt"))
            {
                //reads entire file into one string
                string allFields = reader.ReadToEnd();

                if (allFields[allFields.Length - 1].Equals("\n"))
                {
                    allFields = allFields.Substring(0, allFields.Length - 1);
                }

                //splits that string up into each individual field by taking advantage of tabs
                string[] fields = allFields.Split((char)9, System.StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = fields[i].Trim();
                }

                //if there are't any duplicate fields then add it to the results list
                for (int i = 0; i < fields.Length; i++)
                {
                    Boolean noDuplicate = true;
                    for (int j = i + 1; j < fields.Length; j++)
                    {
                        if (fields[i].Equals(fields[j]))
                        {
                            noDuplicate = false;
                            break;
                        }
                    }
                    if (noDuplicate)
                        result.Add(fields[i]);
                }
            }



            List<string> array = new List<string>();
            foreach (string fieldName in result)
            {
                string value = "";
                if (fieldName.Trim().Substring(0, 1).Equals("\""))
                {
                    value = fieldName.Trim().Substring(1, fieldName.Length - 2);
                    value = value.Replace('\n', ' ');
                }
                else
                {
                    value = fieldName.Trim().Replace('\n', ' ');
                }
                array.Add(value);
            }

            return array;
        }
        public static void GetJsonTemplateMapping(string platform, string type, List<string> array)
        {

            string template = "";
            foreach (string reader in array)
            {
                string fieldName = reader;
                template += $"{{\n\"ElementName\": \"{platform}|{type}|{fieldName}\",\n";
                template += $"\"ElementDispalyName\": \"{fieldName}\",\n";
                template += $"\"MappedStatus\": 0,\n";
                template += $"\"Requirement\": ,\n";
                template += $"\"HelpNum\": ,\n";
                template += "\"Mapper\": {\n";
                template += "\"MapperType\": 1,\n";
                template += "\"MapperLookup\": 0,\n";
                template += "\"AttributeMappers\": []\n},\n";
                template += "\"schemaGroup\": \"\",\n";
                template += "\"jsonPath\": \"\",\n";
                template += "\"IsArray\": ,\n";
                template += "\"IsClosedList\": ,\n";
                template += "\"ParentKey\": \"\",\n";
                template += "\"ConditionalMappers\": []\n},\n";
            }
            using (StreamWriter writer = new StreamWriter("../../../Result.txt")) 
            {
                writer.WriteLine(template);
            }
        }
    }
}
