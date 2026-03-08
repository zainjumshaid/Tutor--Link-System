using System;
using System.Collections.Generic;
using System.IO;

namespace TutorLinkSystem
{
    class Student
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }

        public Student() { }

        public Student(string name, string subject, string contact, string password)
        {
            Name = name;
            Subject = subject;
            Contact = contact;
            Password = password;
        }

        public Student(Student other)
        {
            Name = other.Name;
            Subject = other.Subject;
            Contact = other.Contact;
            Password = other.Password;
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter("students.txt", true))
            {
                sw.WriteLine($"{Name}|{Subject}|{Contact}|{Password}");
            }
        }
    }

    class Tutor
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Qualification { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }

        public Tutor() { }

        public Tutor(string name, string subject, string qualification, string contact, string password)
        {
            Name = name;
            Subject = subject;
            Qualification = qualification;
            Contact = contact;
            Password = password;
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter("tutors.txt", true))
            {
                sw.WriteLine($"{Name}|{Subject}|{Qualification}|{Contact}|{Password}");
            }
        }
    }

    class TutorLinkSystem
    {
        public void Run()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("=======================================");
                Console.WriteLine("          TUTOR LINK SYSTEM");
                Console.WriteLine("=======================================");
                Console.WriteLine("1. Student Registration");
                Console.WriteLine("2. Tutor Registration");
                Console.WriteLine("3. View All Students");
                Console.WriteLine("4. View All Tutors");
                Console.WriteLine("5. Exit");
                Console.WriteLine("----------------------------------------");
                Console.Write("Enter choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice)) choice = 0;

                switch (choice)
                {
                    case 1: StudentRegistration(); break;
                    case 2: TutorRegistration(); break;
                    case 3: ViewAllStudents(); break;
                    case 4: ViewAllTutors(); break;
                    case 5: Console.WriteLine("Exit!"); break;
                    default: Console.WriteLine("Invalid choice!"); break;
                }

                if (choice != 5)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }

            } while (choice != 5);
        }

        private void StudentRegistration()
        {
            Console.WriteLine("\n1. Create Account\n2. Login\nEnter choice: ");
            int option;
            if (!int.TryParse(Console.ReadLine(), out option)) option = 0;

            if (option == 1)
            {
                string name = GetValidInput("Enter Name: ", true);
                string subject = GetValidInput("Enter Subject: ", true);
                string contact = GetValidContact();
                string password = GetValidPassword();

                var student = new Student(name, subject, contact, password);
                student.Save();

                Console.WriteLine("\nAccount created successfully!");
                Console.WriteLine($"\nSearching tutor for {subject}...\n");
                MatchStudentWithTutor(subject);
            }
            else if (option == 2)
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                bool found = false;
                if (File.Exists("students.txt"))
                {
                    foreach (var line in File.ReadAllLines("students.txt"))
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4 && parts[0] == name && parts[3] == password)
                        {
                            found = true;
                            Console.WriteLine("\nLogin successful!");
                            MatchStudentWithTutor(parts[1]);
                            break;
                        }
                    }
                }

                if (!found)
                    Console.WriteLine("\nAccount not found!");
            }
        }

        private void TutorRegistration()
        {
            string name = GetValidInput("Enter Name: ", true);
            string subject = GetValidInput("Enter Subject: ", true);
            Console.Write("Enter Qualification: ");
            string qualification = Console.ReadLine();
            string contact = GetValidContact();
            string password = GetValidPassword();

            var tutor = new Tutor(name, subject, qualification, contact, password);
            tutor.Save();

            Console.WriteLine("\nTutor registered successfully!");
        }

        private void ViewAllStudents()
        {
            if (!File.Exists("students.txt"))
            {
                Console.WriteLine("No students registered yet.");
                return;
            }

            int count = 1;
            Console.WriteLine("\n===== ALL STUDENTS =====");
            foreach (var line in File.ReadAllLines("students.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length == 4)
                {
                    Console.WriteLine($"\nStudent {count++}");
                    Console.WriteLine($"Name: {parts[0]}");
                    Console.WriteLine($"Subject: {parts[1]}");
                    Console.WriteLine($"Contact: {parts[2]}");
                    Console.WriteLine($"Password: {parts[3]}");
                }
            }
        }

        private void ViewAllTutors()
        {
            if (!File.Exists("tutors.txt"))
            {
                Console.WriteLine("No tutors registered yet.");
                return;
            }

            int count = 1;
            Console.WriteLine("\n===== ALL TUTORS =====");
            foreach (var line in File.ReadAllLines("tutors.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length == 5)
                {
                    Console.WriteLine($"\nTutor {count++}");
                    Console.WriteLine($"Name: {parts[0]}");
                    Console.WriteLine($"Subject: {parts[1]}");
                    Console.WriteLine($"Qualification: {parts[2]}");
                    Console.WriteLine($"Contact: {parts[3]}");
                    Console.WriteLine($"Password: {parts[4]}");
                }
            }
        }

        private void MatchStudentWithTutor(string subject)
        {
            bool found = false;
            if (!File.Exists("tutors.txt")) return;

            foreach (var line in File.ReadAllLines("tutors.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length == 5 && parts[1].Equals(subject, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    Console.WriteLine($"Tutor available for {subject}:");
                    Console.WriteLine($"Name: {parts[0]}");
                    Console.WriteLine($"Qualification: {parts[2]}");
                    Console.WriteLine($"Contact: {parts[3]}\n");
                }
            }

            if (!found)
                Console.WriteLine($"\nNo tutor found for {subject}.");
        }

        private string GetValidInput(string prompt, bool onlyLetters)
        {
            string input;
            bool valid;
            do
            {
                valid = true;
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) valid = false;
                if (onlyLetters)
                {
                    foreach (char c in input)
                        if (!char.IsLetter(c) && c != ' ') valid = false;
                }
                if (!valid) Console.WriteLine("Invalid input. Try again.");
            } while (!valid);
            return input;
        }

        private string GetValidContact()
        {
            string contact;
            do
            {
                Console.Write("Enter Contact (11 digits): ");
                contact = Console.ReadLine();
            } while (contact.Length != 11 || !long.TryParse(contact, out _));
            return contact;
        }

        private string GetValidPassword()
        {
            string password;
            do
            {
                Console.Write("Enter Password (max 8 digits, numeric): ");
                password = Console.ReadLine();
            } while (password.Length > 8 || !long.TryParse(password, out _));
            return password;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var system = new TutorLinkSystem();
            system.Run();
        }
    }
}