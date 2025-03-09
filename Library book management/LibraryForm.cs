using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_book_management
{
    public partial class LibraryForm : Form
    {
        private LibraryManager libraryManager;
        private TextBox authorTextBox;
        private TextBox titleTextBox;
        private TextBox yearTextBox;
        private Button addBookButton;
        private Button removeBookButton;
        private TextBox searchTextBox;
        private Button searchButton;
        private ListBox booksListBox;

        public LibraryForm()
        {
            InitializeComponent();
            this.Text = "Управление книгами";
            this.Width = 500;
            this.Height = 400;

            // Author TextBox
            authorTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Width = 150,
                Text = "Автор"
            };
            authorTextBox.Enter += (s, ev) => { if (authorTextBox.Text == "Автор") authorTextBox.Text = ""; };
            authorTextBox.Leave += (s, ev) => { if (string.IsNullOrEmpty(authorTextBox.Text)) authorTextBox.Text = "Автор"; };

            // Title TextBox
            titleTextBox = new TextBox
            {
                Location = new Point(170, 10),
                Width = 150,
                Text = "Название"
            };
            titleTextBox.Enter += (s, ev) => { if (titleTextBox.Text == "Название") titleTextBox.Text = ""; };
            titleTextBox.Leave += (s, ev) => { if (string.IsNullOrEmpty(titleTextBox.Text)) titleTextBox.Text = "Название"; };

            // Year TextBox
            yearTextBox = new TextBox
            {
                Location = new Point(330, 10),
                Width = 80,
                Text = "Год"
            };
            yearTextBox.Enter += (s, ev) => { if (yearTextBox.Text == "Год") yearTextBox.Text = ""; };
            yearTextBox.Leave += (s, ev) => { if (string.IsNullOrEmpty(yearTextBox.Text)) yearTextBox.Text = "Год"; };

            // Add Button
            addBookButton = new Button
            {
                Location = new Point(10, 40),
                Text = "Добавить",
                Width = 100
            };
            addBookButton.Click += AddBookButton_Click;

            // Remove Button
            removeBookButton = new Button
            {
                Location = new Point(120, 40),
                Text = "Удалить",
                Width = 100
            };
            removeBookButton.Click += RemoveBookButton_Click;

            // Search TextBox
            searchTextBox = new TextBox
            {
                Location = new Point(10, 70),
                Width = 200,
                Text = "Поиск"
            };
            searchTextBox.Enter += (s, ev) => { if (searchTextBox.Text == "Поиск") searchTextBox.Text = ""; };
            searchTextBox.Leave += (s, ev) => { if (string.IsNullOrEmpty(searchTextBox.Text)) searchTextBox.Text = "Поиск"; };

            // Search Button
            searchButton = new Button
            {
                Location = new Point(220, 70),
                Text = "Искать",
                Width = 80
            };
            searchButton.Click += SearchButton_Click;

            // Books ListBox
            booksListBox = new ListBox
            {
                Location = new Point(10, 100),
                Width = 450,
                Height = 200
            };

            // Add controls to form
            Controls.Add(authorTextBox);
            Controls.Add(titleTextBox);
            Controls.Add(yearTextBox);
            Controls.Add(addBookButton);
            Controls.Add(removeBookButton);
            Controls.Add(searchTextBox);
            Controls.Add(searchButton);
            Controls.Add(booksListBox);

            libraryManager = new LibraryManager();
            UpdateBooksList();
        }

        private void UpdateBooksList()
        {
            booksListBox.Items.Clear();
            foreach (var book in libraryManager.Books)
            {
                booksListBox.Items.Add($"{book.Author} - {book.Title} ({book.Year})");
            }
        }
        private void AddBookButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(authorTextBox.Text) || string.IsNullOrEmpty(titleTextBox.Text) ||
            string.IsNullOrEmpty(yearTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            Book newBook = new Book(authorTextBox.Text, titleTextBox.Text, yearTextBox.Text);
            try
            {
                libraryManager.AddBook(newBook);
                authorTextBox.Clear();
                titleTextBox.Clear();
                yearTextBox.Clear();
                UpdateBooksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveBookButton_Click(object sender, EventArgs e)
        {
            if (booksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите книгу для удаления!");
                return;
            }
            string selectedItem = booksListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string author = parts[0].Trim();
                string title = parts[1].Trim();
                var bookToRemove = libraryManager.Books.Find(b => b.Author == author && b.Title
                == title);
                if (bookToRemove != null)
                {
                    try
                    {
                        libraryManager.RemoveBook(bookToRemove);
                        UpdateBooksList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                UpdateBooksList();
                return;
            }
            var searchResults = libraryManager.SearchBooks(searchTextBox.Text);
            booksListBox.Items.Clear();
            foreach (var book in searchResults)
            {
                booksListBox.Items.Add($"{book.Author} - {book.Title} ({book.Year})");
            }
        }
    }
}