using FileHandler.Exceptions;

namespace FileHandler
{
    class Program
    {
        static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Users.txt");

        static void Main(string[] args)
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files"));

            Console.WriteLine("Velkommen til FileHandler!");

            while (true)
            {
                try
                {
                    RegisterUser();
                    DisplayUsers();
                }
                catch (InvalidNameException ex)
                {
                    Console.WriteLine($"Fejl: {ex.Message}");
                }
                catch (InvalidAgeException ex) when (ex.Message.Contains("Niels Olesen"))
                {
                    Console.WriteLine("Aldersvalidering sprang over for Niels Olesen.");
                }
                catch (InvalidAgeException ex)
                {
                    Console.WriteLine($"Fejl: {ex.Message}");
                }
                catch (InvalidEmailException ex)
                {
                    Console.WriteLine($"Fejl: {ex.Message}");
                    Console.WriteLine($"Detaljer: {ex.InnerException?.Message}");
                }
                catch (FileLoadException ex)
                {
                    Console.WriteLine($"Filfejl: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("Programmet afsluttes korrekt.\n");
                }
            }
        }

        static void RegisterUser()
        {
            Console.Write("Indtast fornavn og efternavn: ");
            string fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
                throw new InvalidNameException("Navnet må ikke være tomt.");

            Console.Write("Indtast alder: ");
            if (!int.TryParse(Console.ReadLine(), out int age) || age < 18 || age > 50)
            {
                if (fullName.Equals("Niels Olesen", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Alderskontrol ignoreret for Niels Olesen.");
                }
                else
                {
                    throw new InvalidAgeException("Alderen skal være mellem 18 og 50.");
                }
            }

            Console.Write("Indtast e-mail: ");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
                throw new InvalidEmailException("E-mail er ikke gyldig.", new Exception("E-mail mangler @ eller ."));

            AppendUserToFile(fullName, age, email);
        }

        static void AppendUserToFile(string fullName, int age, string email)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    writer.WriteLine($"{fullName}, {age}, {email}");
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Der opstod en fejl under skrivning til filen.", ex);
            }
        }

        static void DisplayUsers()
        {
            try
            {
                if (!File.Exists(filePath))
                    File.Create(filePath).Dispose();

                Console.WriteLine("\nRegistrerede brugere:");
                string[] users = File.ReadAllLines(filePath);
                foreach (string user in users)
                {
                    Console.WriteLine(user);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Kunne ikke læse fra filen.", ex);
            }
        }
    }
}
