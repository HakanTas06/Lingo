namespace Lingo
{
    public partial class Form1 : Form
    {
        int currentTry = 1; //Oyuncunun þu anki deneme hakký (1-5).
        int currentRound = 1; //Oyunun þu anki turu (1-3).
        int timeLeft = 20; //Her denemede kalan süre.
        int totalScore = 0;
        string currentWord = "";
        char[] revealedLetters = new char[5]; //Önceki denemelerden doðru tahmin edilen harflerin tutulduðu dizi.
        List<string> wordList = new List<string> { "ANCAK","ARABA","AYRAÇ","AYRAN","BALIK","BASÝT","BAZEN","BÝDON",
            "BOÐAZ","CEVÝZ","ÇOCUK","DÝVAN","DORUK","ENSAR","ENZÝM","FÝDAN","FÝYAT","GÝTAR","GONCA","HAKÝM",
            "KAÝDE","KEMAL","KÖPEK","KUMAÞ","KUTUP","MELEK","NASIL","NEDÝR","NÝMET","NOHUT",
            "ORTAM","ÖRDEK","PATEN","PÝTON","RANZA","SABÝT","SAÐIR","SAKÝN","SAZAN",
            "SERÝN","YAÐIZ","YAKIN","YEMÝN","YILAN","ZEHÝR","ZEMÝN"};
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTahminEt_Click(object sender, EventArgs e)
        {
            string guess = txtTahmin.Text.ToUpper();
            if (guess.Length != 5)
            {
                MessageBox.Show("5 harfli bir kelime girin.");
                return;
            }

            timer1.Stop();
            KontrolEt(guess);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartRound();
        }
       
        private void StartRound()
        {
            currentWord = wordList[rnd.Next(wordList.Count)];
            currentTry = 1;
            timeLeft = 20;
            Array.Clear(revealedLetters, 0, 5);
            revealedLetters[0] = currentWord[0];

            lblTur.Text = "Tur: " + currentRound;
            lblSkor.Text = "Skor: " + totalScore;
            lblHak.Text = "Hak: 5";
            lblTimer.Text = "Süre: 20 sn";
            txtTahmin.Clear();

            for (int row = 1; row <= 5; row++)
            {
                for (int col = 1; col <= 5; col++)
                {
                    Label lbl = this.Controls.Find($"lbl{row}_{col}", true).FirstOrDefault() as Label;
                    if (lbl != null)
                    {
                        lbl.Text = "-";
                        lbl.BackColor = Color.LightGray;
                    }
                }
            }

            // Ýlk harfi 1. satýra turuncu olarak yaz
            Label lblFirst = this.Controls.Find("lbl1_1", true).FirstOrDefault() as Label;
            if (lblFirst != null)
            {
                lblFirst.Text = currentWord[0].ToString();
                lblFirst.BackColor = Color.Green;
            }

            timer1.Start();
        }
        private void KontrolEt(string guess)
        {
            bool[] matched = new bool[5];
            bool[] used = new bool[5];

            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == currentWord[i])
                {
                    string lblName = $"lbl{currentTry}_{i + 1}";
                    Label lbl = this.Controls.Find(lblName, true).FirstOrDefault() as Label;
                    if (lbl != null)
                    {
                        lbl.Text = guess[i].ToString();
                        lbl.BackColor =Color.Green;
                    }
                    matched[i] = true;
                    used[i] = true;
                    revealedLetters[i] = guess[i];
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (matched[i]) continue;

                string lblName = $"lbl{currentTry}_{i + 1}";
                Label lbl = this.Controls.Find(lblName, true).FirstOrDefault() as Label;
                if (lbl == null) continue;

                lbl.Text = guess[i].ToString();

                for (int j = 0; j < 5; j++)
                {
                    if (!used[j] && guess[i] == currentWord[j])
                    {
                        lbl.BackColor = Color.Orange;
                        used[j] = true;
                        goto Geç;
                    }
                }

                lbl.BackColor = Color.Gray;
            Geç:;
            }

            if (guess == currentWord)
            {
                MessageBox.Show("Tebrikler! Doðru bildiniz. +2000 TL");
                totalScore += 2000;
                lblSkor.Text = "Skor: " + totalScore;
                NextRound();
            }
            else
            {
                NextTry();
            }
        }

        private void NextTry()
        {
            currentTry++;
            if (currentTry > 5)
            {
                MessageBox.Show("5 hakkýnýz bitti. Doðru kelime: " + currentWord);
                NextRound();
            }
            else
            {
                timeLeft = 20;
                lblHak.Text = "Hak: " + (6 - currentTry);
                txtTahmin.Clear();
                timer1.Start();

                for (int i = 0; i < 5; i++)
                {
                    string lblName = $"lbl{currentTry}_{i + 1}";
                    Label lbl = this.Controls.Find(lblName, true).FirstOrDefault() as Label;
                    if (lbl != null)
                    {
                        if (revealedLetters[i] != '\0')
                        {
                            lbl.Text = revealedLetters[i].ToString();
                            lbl.BackColor = Color.Green;
                        }
                        else
                        {
                            lbl.Text = "-";
                            lbl.BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }
        private void NextRound()
        {
            currentRound++;
            if (currentRound > 3)
            {
                MessageBox.Show("Oyun bitti! Toplam kazanç: " + totalScore + " TL");
                Application.Exit();
            }
            else
            {
                StartRound();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = "Süre: " + timeLeft + " sn";

            if (timeLeft <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre doldu!");
                NextTry();
            }
        }
    }
}
