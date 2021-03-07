/*
 *Kevin Liang and Roxxannia Wang
 * June 10, 2019
 * The Zoo assignment
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoxxanniaKevinZooAssignment
{
    class Program
    {
        //declare all the arrays that will be used in the program
        static string[] Names;
        static string[] Descriptions;
        static int[] Quantities;
        static int[] Food;
        static double[] Prices;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            //assign array size
            Names = new string[0];
            Descriptions = new string[0];
            Quantities = new int[0];
            Food = new int[0];
            Prices = new double[0];
            //the user has $7000 from the very beginning
            double money = 7000.0;
            //set the inital day count to be 1 
            int daycount = 1;
            //set up the pre-existing zoo with subprogram CreateStarterZoo
            CreateStarterZoo();
            //run menu subprogram to carry out all the functions
            Menu(money, daycount);
        }

        /*Kevin Liang START */

        static string AnimalLove()
        {
            // create a random int between 0 and 100
            int randomInt = rnd.Next(0, 100);
            // loops through all different quantities pertaining to different habitats
            for (int i = 0; i < Quantities.Length; i++)
            {
                // if random int is less than x5 the quantity AND the max number of animals
                // has not been reached, add 1 animal to the habitat and return a string saying the animal has an increase in population
                if (randomInt < 5 * Quantities[i] && Quantities[i] > 10000 / Prices[i])
                {
                    Quantities[i] = Quantities[i] + 1;
                    string increase = Names[i] + " had an increase in population!";
                    return increase;
                }

                else
                {
                    // else, do not increase population of that habitat and return a string saying the animal had no increase in population
                    string noIncrease = Names[i] + " had no increase in poulation.";
                    return noIncrease;
                }
            }

            return "";
        }

        // checks if there is enough food in the habitat for the quantity of animals
        static string AnimalsEat()
        {
            // loops through all habitats' food and quantities
            for (int i = 0; i < Food.Length; i++)
            {
                // if food available is more than quantities of animals, all animals are fed
                // and food is decreased by quantity of animals in the habitat.
                // a string is returned saying all animals were fed.
                if (Food[i] > Quantities[i])
                {
                    Food[i] = Food[i] - Quantities[i];
                    string fed = "All animals were fed.";
                    return fed;

                }

                // if food available is less than quantities of animals, all food is depleted
                // and the number of animals eaten is the number of unfed animals divided by three
                else if (Food[i] < Quantities[i])
                {
                    Food[i] = 0;
                    int unfed = Quantities[i] - Food[i];
                    int eaten = unfed / 3;

                    // a minimum of 1 animal is eaten if there is not enough food, quantities is
                    // depleted by one and a string is returned saying one of which animal was eaten
                    if (eaten < 1)
                    {
                        Quantities[i] = Quantities[i] - 1;
                        string oneEaten = "1 " + Names[i] + " was eaten.";
                        return oneEaten;
                    }

                    // decrease the quantity of the habitat by amount of animals eaten,
                    // return string saying how many of which animals were eaten.
                    Quantities[i] = Quantities[i] - eaten;
                    string amountEaten = eaten + " " + Names[i] + "(s) were eaten.";
                    return amountEaten;
                }
            }

            return "";
        }

        // Builds new habitat so a new type of animal can be added
        static string BuildNewHabitat(string name, string description, double price, ref double money)
        {
            // searches to see if the animal type is already present in the zoo
            int nameSearch = ListFindExactIgnoreCase(Names, name);

            // if the search is successful, return string that says the animal type is already present in the zoo
            if (nameSearch != -1)
            {
                string duplicate = "This animal habitat already exists.";
                return duplicate;
            }

            // if animal type is new AND user has less than $2000, return string saying they do not have enough money
            else if (nameSearch == -1 && money < 2000)
            {
                string notEnoughMoney = "You need $2000 to build a new habitat.";
                return notEnoughMoney;
            }

            // if animal type is new AND user has enough money AND there are already 10
            // animal types in the zoo, return string saying the zoo is maxed out
            else if (nameSearch == -1 && money > 2000 && Names.Length == 10)
            {
                string full = "The zoo is at max capacity.";
                return full;
            }

            // if all conditions are satisfied, build the new habitat by adding the name, price,
            // and description of the new animal type into the parallel arrays.
            // decrease money by 2000.
            else if (nameSearch == -1 && money > 2000 && Names.Length < 10)
            {
                ListAdd(ref Names, name);
                ListAdd(ref Prices, price);
                ListAdd(ref Descriptions, description);
                ListAdd(ref Quantities, 0);
                ListAdd(ref Food, 0);
                money = money - 2000;
                string habitatAdded = "The " + name + " habitat was sucessfully created.";
                return habitatAdded;
            }

            return "";
        }

        // buy an animal for an existing habitat in the zoo
        static string BuyExistingAnimal(double moneyAvailable, string name)
        {
            // searches for existing name of the animal
            int animal = ListFindExactIgnoreCase(Names, name);

            // if there are no results, return string saying the animal does not exist
            if (animal == -1)
            {
                string animalNotFound = "That animal does not exist";
                return animalNotFound;
            }

            // if search is successful AND the user does not have enough money,
            // return string saying they do not have enough money
            else if (animal != -1 && moneyAvailable < Prices[animal])
            {
                string notEnoughMoney = "You do not have enough money";
                return notEnoughMoney;
            }

            // if search is successful and user has enough money, reduce money by price of
            // the animal and add one to its quantity
            else if (animal != -1 && moneyAvailable > Prices[animal])
            {
                moneyAvailable = moneyAvailable - Prices[animal];
                Quantities[animal] = Quantities[animal] + 1;
                string purchaseSuccess = "You have successfully purchased " + name + "\n" + "Money left: " + moneyAvailable;
                return purchaseSuccess;
            }

            return "";
        }

        // calculates random amount of money earned per each animal type
        static double EarnMoney()
        {
            // creates random decimal between 0.5 and 1.5
            double randomDouble = rnd.NextDouble() + 0.5;
            double moneyEarned = 0;

            // loops through each animal type while applying the earn money formula while adding the resulting
            // money earned per animal type into the total money earned
            for (int i = 0; i > Quantities.Length; i++)
            {
                double moneyPerAnimalType = randomDouble * Quantities[i] * (Prices[i] / 50.0);
                moneyEarned = moneyEarned + moneyPerAnimalType;
            }

            // return total amount earned
            return moneyEarned;
        }

        // ends the day
        static string EndTurn(ref double money, ref int dayCount)
        {
            // increases day count by 1 and returns string showing which day the zoo is on,
            // if all animals were fed or if some died of starvation, if any new animals were born
            // and the amount of money the user has
            dayCount = dayCount + 1;
            string endTurn = "You are now on day: " + dayCount + "\n" + AnimalsEat() + "\n" + AnimalLove() + "\n" + "You now have: $" + money + EarnMoney();
            return endTurn;
        }
        // ensures the user does not input an empty string
        static string InputNotEmpty(string prompt)
        {
            string output = "";

            do
            {
                Console.Write(prompt);
                output = Console.ReadLine();
            } while (output == "");

            return output;
        }

        // ensures the user inputs a number between the specified minimum and maximum integers
        static double InputNumber(int minimum, int maximum)
        {
            int price = 0;

            do
            {
                Console.Write("Input Price between 50 - 500: ");
                int.TryParse(Console.ReadLine(), out price);
            } while (price < minimum || price > maximum);

            return price;
        }

        // search using keywords that may be found in the descriptions of animals
        static string SearchByDescription(string toFind)
        {
            // create array with appropriate size to store results
            int[] results = ListFindPartial(Descriptions, toFind);
            string outcome = "";
            // if the array is size 0, return string saying no results were found.
            if (results.Length == 0)
            {
                outcome = "No results were found with the following description.";
            }

            // loop through the different results and print out the animals' information
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    outcome = outcome + "\r\n" + FormatAnimalData(results[i]) + "\r\n";
                }
            }
            return outcome;

        }

        // show all animal data in order of descending prices
        static string[] ShowDescendingByPrice()
        {
            // create identical copies of the original parallel arrays
            string[] sNames = new string[Names.Length];
            string[] sDescriptions = new string[Descriptions.Length];
            int[] sQuantities = new int[Quantities.Length];
            int[] sFood = new int[Food.Length];
            double[] sPrices = new double[Prices.Length];

            for (int i = 0; i < Names.Length; i++)
            {
                sNames[i] = Names[i];
                sDescriptions[i] = Descriptions[i];
                sQuantities[i] = Quantities[i];
                sFood[i] = Food[i];
                sPrices[i] = Prices[i];
            }

            // repeatedly moves the next lowest value to the end of the arrays
            for (int k = 0; k < Prices.Length; k++)
            {
                // stops looping 1 element sooner each time it loops
                for (int i = 0; i < Prices.Length - 1 - k; i++)
                {
                    // compares side-by-side values; if right one is bigger then swap with left one
                    if (sPrices[i] < sPrices[i + 1])
                    {
                        string tempNames = sNames[i];
                        string tempDescriptions = sDescriptions[i];
                        int tempQuantities = sQuantities[i];
                        int tempFood = sFood[i];
                        double tempPrices = sPrices[i];
                        sNames[i] = sNames[i + 1];
                        sDescriptions[i] = sDescriptions[i + 1];
                        sQuantities[i] = sQuantities[i + 1];
                        sFood[i] = sFood[i + 1];
                        sPrices[i] = sPrices[i + 1];
                        sNames[i + 1] = tempNames;
                        sDescriptions[i + 1] = tempDescriptions;
                        sQuantities[i + 1] = tempQuantities;
                        sFood[i + 1] = tempFood;
                        sPrices[i + 1] = tempPrices;
                    }
                }
            }

            // make new array and store all sorted information into the array, return the array
            string[] result = new string[sPrices.Length];
            for (int i = 0; i < sPrices.Length; i++)
            {
                result[i] = FormatAnimalData(i, sNames, sDescriptions, sPrices, sQuantities, sFood);
            }

            return result;
        }

        // increases the double array size by one
        static void IncreaseArraySide(ref double[] data)
        {
            // creates a copy one element larger than original
            double[] copy = new double[data.Length + 1];

            // copy all data in from original, set original equal to the larger copy
            for (int i = 0; i < data.Length; i++)
            {
                copy[i] = data[i];
            }

            data = copy;
        }

        // increases the int array size by one
        static void IncreaseArraySide(ref int[] integer)
        {
            int[] copy = new int[integer.Length + 1];

            for (int i = 0; i < integer.Length; i++)
            {
                copy[i] = integer[i];
            }

            integer = copy;
        }

        // increases the string array size by one
        static void IncreaseArraySide(ref string[] data)
        {
            string[] copy = new string[data.Length + 1];

            for (int i = 0; i < data.Length; i++)
            {
                copy[i] = data[i];
            }

            data = copy;
        }

        // adds a decimal to the end of the double array
        static void ListAdd(ref double[] array, double toAdd)
        {
            IncreaseArraySide(ref array);
            array[array.Length - 1] = toAdd;
        }

        // adds an integer to the end of the integer array
        static void ListAdd(ref int[] array, int toAdd)
        {
            IncreaseArraySide(ref array);
            array[array.Length - 1] = toAdd;
        }

        // adds a string to the end of the string array
        static void ListAdd(ref string[] array, string toAdd)
        {
            IncreaseArraySide(ref array);
            array[array.Length - 1] = toAdd;
        }

        static int[] ListFindPartial(string[] array, string toFind)
        {
            int numberOfResults = 0;
            // loops through all array elements, everytime it contains the search toFind
            // increase counter by 1
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].ToLower().Contains(toFind.ToLower()))
                {
                    numberOfResults++;
                }
            }

            // create a new array with size that corresponds to the number of results
            int[] indexes = new int[numberOfResults];
            int index = 0;

            // copy all indexes of the matching elements of the array into the new array
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].ToLower().Contains(toFind.ToLower()))
                {
                    indexes[index] = i;
                    index++;
                }
            }

            return indexes;
        }



        /*Kevin Liang END */

        /*Roxxannia Wang Start*/

        //this subprogram is to display information of the animals in each habitat using the format
        static string CheckAllHabits()
        {
            //create a string to store the formated animal data
            string allHabitats = "";
            for (int i = 0; i < Names.Length; i++)
            {
                allHabitats = allHabitats + "\r\n" + FormatAnimalData(i) + "\r\n";
            }
            return allHabitats;
        }

        //this subprogram is to establish 3 pre-existing habitats
        static void CreateStarterZoo()
        {
            //the player starts with $7000.0
            double money = 7000.0;
            //build 3 new habitats with their names, descriptions, prices. 
            //these three new habitats do not counter towards the total money that the user starts with
            BuildNewHabitat("Tiger", "King", 400.0, ref money);

            BuildNewHabitat("Duck", "Water", 60.0, ref money);

            BuildNewHabitat("Pikachu", "Pokemon", 490.0, ref money);
        }

        //This subprogram is to feed aninal by passing reference of money available
        static string FeedAnimals(ref double moneyAvaible)
        {
            //declare a string to store information
            string feedAnimals = "";
            //loop through the entire array of prices
            for (int i = 0; i < Prices.Length; i++)
            {
                //the actual price of food for each animal is a quarter of their original price
                double price = Prices[i] * 0.25;
                //the total price of food for all the animals in one habitat equals to the unit price times the quantities of that animal
                double totalPrices = price * (double)Quantities[i];
                //if the total prices is greater than money Available, feed that animals that the player can afford with the money left, 
                //the amount of food is added by the affordable portion, the animals that are not fed will be killed
                //the afforded portion is subtracted from the money left
                if (totalPrices > moneyAvaible)
                {
                    int foodavailable = (int)moneyAvaible / (int)totalPrices;
                    Food[i] += foodavailable;
                    feedAnimals = "The" + Names[i] + "will not be fully fed";
                    moneyAvaible -= foodavailable * price;
                }
                //otherwise, the money is enough to buy food for all animals
                else
                {
                    //food is increased with the quantities
                    Food[i] += Quantities[i];
                    //the total food expense is subtracted from the money available
                    moneyAvaible -= totalPrices;
                    feedAnimals = "All the animals will be fed";
                }
            }
            return feedAnimals;
        }

        //this subprogram is to format the animal information in eahc habitat, using the parameter of index
        //display animal information in the format of Name:, Description:, Prices:, Quantities:, and Food:
        static string FormatAnimalData(int index)
        {
            //declare string outcomr to store the formatted animal information
            string data = "Name: " + Names[index] +
            "\r\n" + "Description: " + Descriptions[index] +
            "\r\n" + "Prices: " + Prices[index] +
            "\r\n" + "Quantities: " + Quantities[index] +
            "\r\n" + "Food: " + Food[index];

            return data;
        }

        //this subprogram is to format the animal information in eahc habitat, using the parameter of index, sNames string, sDescription string
        //sPrices double array, sQauntities int array, sFood int array;
        //display animal information in the format of Name:, Description:, Prices:, Quantities:, and Food:
        static string FormatAnimalData(int index, string[] sNames, string[] sDescriptions, double[] sPrices, int[] sQuantities, int[] sFood)
        {
            //declare string outcomr to store the formatted animal information            
            string format = "Name: " + sNames[index] +
            "\r\n" + "Description: " + sDescriptions[index] +
            "\r\n" + "Prices: " + sPrices[index] +
            "\r\n" + "Quantities: " + sQuantities[index] +
            "\r\n" + "Food: " + sFood[index];
            return format;
        }

        //this subprogram is the Main menu of this Zoo game 
        //user can choose their desired program by entering corresponding number
        static void Menu(double Money, int dayCount)
        {
            //create a string to store user input
            string menuOption;
            //create outcome string to store information in string format
            string name = "";
            string outcome = "";
            //create an array to store information that is displayed in array format
            string[] show = new string[0];
            //run the following functions if the input is not "11" which exits the program
            do
            {
                //Display all the basica information of the user and the menu options
                Console.WriteLine("Welcome to day " + dayCount + " Of the zoo");
                Console.WriteLine("You currently have $" + Money);
                Console.WriteLine("Menu Option: ");
                Console.WriteLine("0) End the day");
                Console.WriteLine("1) Feed Animals");
                Console.WriteLine("2) Buy Animals for Existing Habitat");
                Console.WriteLine("3) Sell Animal for Existing Habitat");
                Console.WriteLine("4) Build New Habitat for Animals($2000)");
                Console.WriteLine("5) Destroy Existing Habitat($500)");
                Console.WriteLine("6) Check All Habitats");
                Console.WriteLine("7) Check Habitat by Animal Quantity");
                Console.WriteLine("8) Check Habitat by Animal Description Search");
                Console.WriteLine("9) Show Animas by Ascending Quantities");
                Console.WriteLine("10) Show Animals by Descending Price");
                Console.WriteLine("11) Quit Zoo Program");
                Console.WriteLine("Choose a function program");
                menuOption = Console.ReadLine();
                //convert user input to ineger
                int option;
                int.TryParse(menuOption, out option);
                //if user input is 0, it will end the day and does all the deficit and profit calculation
                if (option == 0)
                {
                    outcome = EndTurn(ref Money, ref dayCount);

                }
                //if user input is 1, feed animals function will run
                else if (option == 1)
                {
                    outcome = FeedAnimals(ref Money);

                }
                //if user input is 2, the user needs to input an animal name to buy in the existing habitat
                else if (option == 2)
                {
                    Console.WriteLine("Name of animal to buy: ");
                    string tobuy = Console.ReadLine();
                    outcome = BuyExistingAnimal(Money, tobuy);

                }
                //if user input is 3, the user needs to input an animal name to sell in the existing habitat
                else if (option == 3)
                {
                    Console.WriteLine("Name of animal to sell: ");
                    string tosell = Console.ReadLine();
                    outcome = SellExistingAnimal(ref Money, tosell);

                }
                //if user input is 4, the user needs to input an animal name to build a new habitat. The animal cannot be repeated with 
                //the ones that is already in the zoo
                else if (option == 4)
                {
                    Console.WriteLine("Building new habitat");
                    name = InputNotEmpty("Enter the name of animal you want to build habitat for: ");
                    //check if the animal is already in the program
                    //if true, this new habitat cannot be added
                    if (ListFindExactIgnoreCase(Names, name) != -1)
                    {
                        Console.WriteLine("The animal habitat has already existed");
                    }
                    //if false, the user needs to enter the price of the new animal and give a price for this animal
                    else
                    {
                        outcome = BuildNewHabitat(name, InputNotEmpty("The description is: "), InputNumber(50, 500), ref Money);
                    }

                }
                //if user input is 5, the user needs to choose a habitat to destroy
                else if (option == 5)
                {
                    Console.WriteLine("Choose a habitat to destroy: ");
                    name = Console.ReadLine();
                    outcome = RemoveExistingHabitat(Money, name);

                }

                //if user input is 6, all the habitat information will be displayed
                else if (option == 6)
                {
                    Console.WriteLine("Show all habitat information");
                    outcome = CheckAllHabits();

                }

                //if user input is 7, the user needs to input an integer to search for the habitats that have at least that many animals in
                //display the habitat information if a match is found
                else if (option == 7)
                {
                    Console.Write("Searching for animal by quantity...");
                    Console.Write("Input the minimum quantity to search for: ");
                    int minQuantity;
                    int.TryParse(Console.ReadLine(), out minQuantity);
                    outcome = SearchByQuantity(minQuantity);
                }

                //if user input is 8, the user needs to input a keyword to search for the habitats that contains the matched keywords
                else if (option == 8)
                {
                    Console.Write("Searching for animal by description...");
                    Console.Write("Input the keyword(s) to search for: ");
                    string keyword = Console.ReadLine();
                    outcome = SearchByDescription(keyword);

                }

                //if user input is 9, the animals information will be displayed by ascending quantities order
                else if (option == 9)
                {
                    Console.WriteLine("Show animals by ascending quantities");
                    show = ShowAscendingByQuantities();
                    for (int i = 0; i < Quantities.Length; i++)
                    {
                        outcome = outcome + "\r\n" + show[i] + "\r\n";
                    }

                }

                //if user input is 10, the animals information will be displayed by descending prices order
                else if (option == 10)
                {
                    Console.WriteLine("Show animals by descending prices");
                    show = ShowDescendingByPrice();
                    for (int i = 0; i < Prices.Length; i++)
                    {
                        outcome = outcome + "\r\n" + show[i] + "\r\n";
                    }
                }

                //if user input is 11, the program is ended, Goodbye will be displayed
                else if (option == 11)
                {
                    Console.WriteLine("Bye, good game");
                }

                else
                {
                    Console.WriteLine("Invalid choice selected.");
                }

                //as long as the input is not 11, the user can press any key to continue on other function when one is finished, 
                //previous contexts are all cleared
                if (menuOption != "11")
                {
                    Console.WriteLine(outcome);
                    outcome = "";
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (menuOption != "11");

        }

        //this subporgram is to remove existing habitat as long as the habitat exists in the program, cost $500 each
        static string RemoveExistingHabitat(double Money, string Name)
        {
            //declare int variable to store the index where the name is founded in the Names array
            int Removal = ListFindExactIgnoreCase(Names, Name);
            //declare string to store the result/outcome
            string outcome = "";
            //if the user input is not found, then there is no habitat to remove
            if (Removal == -1)
            {
                outcome = "There is no such habitat to be removed";
            }
            //otherwise
            else
            {
                //if the money left is less than 500, then the user cannot remove the habitat since he/she does not have enough money
                if (Money < 500.0)
                {
                    outcome = "You don't have enough money to remove this habitat";
                }
                //otherwise, the habitat is removed.
                //if there are animals in the habitat, they are sold by their price
                //displayed the removed animal information after its removal
                //corresponding information are removed from the 5 parallel arrays with the same index
                else
                {
                    string removed = "";
                    if (Quantities[Removal] > 0)
                    {
                        double RemoveAnimal = (double)Quantities[Removal] * Prices[Removal];
                        removed = Quantities[Removal] + "of" + Names[Removal] + " are removed and sold for $" + RemoveAnimal;
                        Money += RemoveAnimal;
                    }
                    //total money is decreased by 500
                    Money -= 500;
                    //delete the habitat information from all the array using ListDelete subprogram , referencing to the corresponding array and index
                    ListDelete(ref Names, Removal);
                    ListDelete(ref Descriptions, Removal);
                    ListDelete(ref Quantities, Removal);
                    ListDelete(ref Food, Removal);
                    ListDelete(ref Prices, Removal);
                    removed = "You have successfully removed a habitat and you now have $" + Money;
                    outcome = removed;
                }
            }
            return outcome;
        }

        //this program is to search animals by their minimum quantities in the habitats
        static string SearchByQuantity(int Minimum)
        {
            //create an int array to store all the animals with minimum quantities
            int[] Searchresult = ListFindAtLeast(Quantities, Minimum);
            string outcome = "";
            //if the array's length is 0, then no animal is found with the inputed minimum quantities
            if (Searchresult.Length == 0)
            {
                outcome = "No animal habitat is found with" + Minimum + "animals";
            }
            //otherwise, if there is animal found
            else
            {
                //loop through int array that stored the matched resutls
                for (int i = 0; i < Searchresult.Length; i++)
                {
                    //display the matched animals' information using the format
                    outcome = outcome + "\r\n" + FormatAnimalData(Searchresult[i]) + "\r\n";
                }
            }
            return outcome;
        }

        //this subprogram is to sell exisitng animal in the habitats
        static string SellExistingAnimal(ref double moneyAvailable, string Name)
        {
            //declare int variable to store the index where the name is founded in the Names array
            int SellAnimal = ListFindExactIgnoreCase(Names, Name);
            string outcome = "";
            //if the animal habitat exists
            if (SellAnimal != -1)
            {
                //if there is no animals in the habitat, then there is nothing to be sold
                if (SellAnimal == 0)
                {
                    outcome = "There is no animal can be sold";
                }
                //if there is animals in the habitat
                //decrease the animals quantities, add the money to money available which is the quantities of the animal times the price
                else
                {
                    Quantities[SellAnimal]--;
                    moneyAvailable += Quantities[SellAnimal] * Prices[SellAnimal];
                    outcome = Names[SellAnimal] + " was sold for $" + Prices[SellAnimal] + "\r\n" + "You now have $" + moneyAvailable;
                }
            }
            //otherwise, if the find program returns -1. that means the animal does not exist in the program at all
            else
            {
                outcome = "Animal cannot be found";
            }
            return outcome;
        }

        //this program is to sort animals by their quantities in ascending order
        static string[] ShowAscendingByQuantities()
        {
            //create temporary arrays that is the same length as the original arrays
            string[] tempNames = new string[Names.Length];
            string[] tempDescriptions = new string[Names.Length];
            int[] tempQuantities = new int[Names.Length];
            int[] tempFood = new int[Names.Length];
            double[] tempPrices = new double[Names.Length];

            //copy the objects in the orignal arrays to the temperary array, so that the original information does not get lost
            for (int a = 0; a < Names.Length; a++)
            {
                tempNames[a] = Names[a];
                tempDescriptions[a] = Descriptions[a];
                tempQuantities[a] = Quantities[a];
                tempFood[a] = Food[a];
                tempPrices[a] = Prices[a];
            }
            //loops the program inside that repeadtedly moves the bigger value that is bigger in 2 neighbour objects to the end side    
            //use names length -1 -k to loop without always going back from the firt element
            //the first element will automaticallly be sorted when the second elememnt gets sorted
            for (int k = 0; k < Names.Length; k++)
            {
                //swap the bigger element in two neighbour elemts to the end of the array 
                //we stop looping at "tosort.length - 1 -k" 
                //each time looping througth program, we stop looping 1 element early
                //since every time the soring is finished, one more element is sorted 
                //prevent always going back the very first one to sort again
                for (int i = 0; i < Names.Length - 1 - k; i++)
                {
                    //if the proceeding element is greater than the follwing element
                    if (tempQuantities[i] > tempQuantities[i + 1])
                    {
                        //temperorailly store the prior element
                        string Names1 = tempNames[i];
                        string Descriptions1 = tempDescriptions[i];
                        int Quantities1 = tempQuantities[i];
                        int Food1 = tempFood[i];
                        double Prices1 = tempPrices[i];
                        //make the proceeding one equal to the smaller, following element
                        tempNames[i] = tempNames[i + 1];
                        tempDescriptions[i] = tempDescriptions[i + 1];
                        tempQuantities[i] = tempQuantities[i + 1];
                        tempFood[i] = tempFood[i + 1];
                        tempPrices[i] = tempPrices[i + 1];
                        // make the following one equal to the prior element that is stored temperarily
                        tempNames[i + 1] = Names1;
                        tempDescriptions[i + 1] = Descriptions1;
                        tempQuantities[i + 1] = Quantities1;
                        tempFood[i + 1] = Food1;
                        tempPrices[i + 1] = Prices1;
                    }
                }
            }
            //after sorting, the information of each type of animals is displayed in the format with asceding order in quantities
            string[] result = new string[tempQuantities.Length];
            for (int b = 0; b < tempQuantities.Length; b++)
            {
                result[b] = FormatAnimalData(b, tempNames, tempDescriptions, tempPrices, tempQuantities, tempFood);
            }
            return result;
        }
        //decrease  array size by 1, delete the last element in the double array, shifting the array towards the beginning by 1 
        static void DecreaseArraySize(ref double[] data)
        {
            //resize the array
            double[] temp = new double[data.Length - 1];
            for (int i = 0; i < temp.Length; i++)
            {
                //copy data to the smaller array
                temp[i] = data[i];
            }
            data = temp;
        }
        //decrease  array size by 1, delete the last element in the int array, shifting the array towards the beginning by 1 
        static void DecreaseArraySize(ref int[] data)
        {
            //resize the array
            int[] temp = new int[data.Length - 1];
            for (int i = 0; i < temp.Length; i++)
            {
                //copy data to the smaller array
                temp[i] = data[i];
            }
            //set the data array to the smaller copy
            data = temp;
        }

        //decrease  array size by 1, delete the last element in the string array, shifting the array towards the beginning by 1 
        static void DecreaseArraySize(ref string[] data)
        {
            //resize the array
            string[] temp = new string[data.Length - 1];
            for (int i = 0; i < temp.Length; i++)
            {
                //copy data to the smaller array
                temp[i] = data[i];
            }
            //set the data array to the smaller copy
            data = temp;


        }

        //find the element's location in the doube array at 'indextodelete"
        //if it is found, then that element is returned and deleted at the indextodelete
        //the size of the array decreases  by 1
        //elements after the 'indextodelete wil be be shift 1 (decreae by 1) and move towards the beginning
        static double ListDelete(ref double[] array, int indexToDelete)
        {
            double outcome;
            //check if the index i valid
            if (indexToDelete < 0 || indexToDelete > array.Length)
            {
                outcome = 0.0;
            }
            else
            {
                double tobeDelete = array[indexToDelete];
                //shift info by decreasing the index by 1
                for (int i = indexToDelete; i < array.Length - 1; i++)
                {
                    array[i] = array[i + 1];
                }
                DecreaseArraySize(ref array);
                outcome = tobeDelete;
            }
            return outcome;
        }

        //find the element's location in the doube array at 'indextodelete"
        //if it is found, then that element is returned and deleted at the indextodelete
        //the size of the array decreases  by 1
        //elements after the 'indextodelete wil be be shift 1 (decreae by 1) and move towards the beginning
        static int ListDelete(ref int[] data, int indexToDelete)
        {
            int outcome;
            //check if the index is valid
            if (indexToDelete < 0 || indexToDelete > data.Length)
            {
                outcome = 0;
            }
            else
            {
                int tobeDelete = data[indexToDelete];
                //shift info by decreasing the index by 1
                for (int i = indexToDelete; i < data.Length - 1; i++)
                {
                    data[i] = data[i + 1];
                }
                DecreaseArraySize(ref data);
                outcome = tobeDelete;
            }
            return outcome;
        }


        //find the element's location in the doube array at 'indextodelete"
        //if it is found, then that element is returned and deleted at the indextodelete
        //the size of the array decreases  by 1
        //elements after the 'indextodelete wil be be shift 1 (decreae by 1) and move towards the beginning
        static string ListDelete(ref string[] data, int indexToDelete)
        {
            string outcome;
            //check if the index is valid
            if (indexToDelete < 0 || indexToDelete > data.Length)
            {
                return "";
            }
            else
            {
                string tobeDelete = data[indexToDelete];
                //shift info by decreasing the index by 1
                for (int i = indexToDelete; i < data.Length - 1; i++)
                {
                    data[i] = data[i + 1];
                }
                DecreaseArraySize(ref data);
                outcome = tobeDelete;
            }
            return outcome;
        }

        //the subprogram is to find the corresponding element that has the minimum
        static int[] ListFindAtLeast(int[] array, int Minimum)
        {
            int counter = 0;
            //increas the counter by 1 if an element in the array is found greater than minimum by looping through the array
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= Minimum)
                {
                    counter++;
                }
            }
            //create an int array and resize it with the size of counter which is how many elements are found greater than  minimum
            int[] Min = new int[counter];
            //reset the counter to 0
            counter = 0;
            //loop through the Min array, if the element in the original array is found greater than the minimum, add tho the min array
            for (int k = 0; k < Min.Length; k++)
            {
                if (array[k] >= Minimum)
                {
                    Min[counter] = k;
                    counter++;
                }
            }
            return Min;
        }

        //the subprogram is to find the index of the keywords but ignore the case
        static int ListFindExactIgnoreCase(string[] array, string toFind)
        {
            int outcome = 0;
            //loop through the array, find if the toFind is in the array, return the index if it is found
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i].ToUpper() == toFind.ToUpper())
                {
                    outcome = i;
                    return outcome;
                }
            }
            //return -1 if there's no result
            outcome = -1;
            return outcome;
        }
    }
}
/*Roxxannia wang END*/
