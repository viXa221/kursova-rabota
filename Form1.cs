using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursova_rabota
{
    public partial class Form1 : Form
    {
            //Деклариране на структурите
        private List<Book> books = new List<Book>();// това е основният списък (за съхранение на всички книги)
        private Stack<Book> lastAddedBooks = new Stack<Book>();//това е списъка за отмяна на последно добавената книга
        public Form1()
        {
            InitializeComponent();
        }

        public class Book//създавам отделен клас Book, който дефинира стуктурта от данни на една книга със свойставата долу:
        {
            public string Title { get; set; }//това са свойствата на книгата (property)
            public string Author { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }

            public override string ToString()

                //с това поакзвам книгата в четим вид в списъка
            {
                return $"{Title} е написан от: {Author}, през ({Year}) година. Жанра е - {Genre}";//когато книгата е добавена в listBox-a, излиза това съобщение       
            }//Класът Book има метод ToString и затова книгата ще се покаже така
        }
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string genre = txtGenre.Text.Trim();
            int year;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(genre))
            //проверка на празни полета
            {
                MessageBox.Show("Моля, попълни всики полета");//ако някое поле е празно
                return;
            }
            if (!int.TryParse(txtYear.Text, out year))//опитва се да превърне txtYear в цяло число
              //проверка, дали годината е число
            {
                MessageBox.Show("Годината трябва да е число!");
                return;
            }
            
       

                Book newBook = new Book//създавам си нов обект от класа Book
                {//създава нова книга с въведените стойности, 
                    Title = title,
                    Author = author,
                    Genre = genre,
                    Year = year
                    //стойностите, които всяка книга има
                };
            books.Add(newBook);//добавяме новата книга в списъка. books е от List списъка
            lastAddedBooks.Push(newBook);//това е от Стек структура и се добавя И тук, за да можем когато решим да я премахнем (работи като Undo)

            UpdateBookList();
            ClearTextFields();

            MessageBox.Show("Книгата е добавена успешно!");
            //return;
        }
       

        private void btnDelete_Click(object sender, EventArgs e)//изтриване на книга
        {
            int selectedIndex = listBox1.SelectedIndex;//взима индекса на избрания елемент от списъка
            if(selectedIndex >= 0 && selectedIndex < books.Count)//проверява дали елемента съществува, ако е валиден избраноя индекс
            {
                books.RemoveAt(selectedIndex);//се премахва книгата от списъка books, на съответния индексс
                UpdateBookList();// обновяваме
            }
            else//ако не сме избрали книга
            {
                MessageBox.Show("Моля, изберете книга от списъка!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTitle = txtTitle.Text.Trim().ToLower();//търсене на книга по заглавие
            Book found = books.Find(b => b.Title.ToLower().Contains(searchTitle));
            if(found != null)//
            {
                MessageBox.Show($"Намерена книга:\n{found}");
            }
            else//ако е null, книгата не е намерена:
            {
                MessageBox.Show("Книгата със зададеното заглавие не е намерена");
            }
        }

        private void btnRemoveLast_Click(object sender, EventArgs e)
        {
            if(lastAddedBooks.Count > 0)
                // проверява дали има добавена книга за премахване
            {
                Book last = lastAddedBooks.Pop();//премахваме последната добавена книга в списъка
                //тук използвам и двете структури от данни, защото в случая е за изтриване на книга,
                //когато изтривам книга, тя е била запазена и в двете структури,
                //затова се взима по индекс от List-a, и после от Stack-a.
                //тук Book е от списъка на List
                //проверява коя е последно добавената книга
                books.Remove(last);//книгата се премахва
                UpdateBookList();//и BookList-а се рефрешва
            }
            else//ако няма добавени книги
            {
                MessageBox.Show("Няма последно обавена книга за отмяна");
            }
        }
        private void UpdateBookList()
        {
            listBox1.Items.Clear();//изтрива всички ккниги, които са в ListBox-a
            foreach(Book b in books)//обхождаме всички книги 
            {
                listBox1.Items.Add(b);//добавяме всяка книга в списъка (ListBox1)
            }
        }
        private void ClearTextFields()//
            //декларирам си метод, който да изчиства полетата, когота книгата се добави
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtGenre.Clear();
            txtYear.Clear();
        }
    }
}
