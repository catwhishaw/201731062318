using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program3
{
    class Word
    {
        public string text;
        public int num;
        public Word(string text)
        {
            this.text = text;
            num = 1;
        }
    }
    public class Program
    {
        List<string> Lines = new List<string>();//从文档读取到的文本内容
        List<Word> Words = new List<Word>();//检索到的单词
        List<Word> MutipleWords = new List<Word>();//检索到的词组

        List<string> SaveStrs = new List<string>();

        public int lines = 0;//有效行数
        public int characters = 0;//有效字符数

        public static int number;

        public static int outPutNum;

        public static string path;
        public static string s_path;

        public static Program program = new Program();

        static void Main(string[] args)
        {
            program.TestMethod();//调用函数
            Console.Read();
        }

        public void TestMethod()
        {
            program.DataInput();//输入
            program.FileRead(path);//文件读取
            program.MainCount();//计数
            program.Output();//输出
            program.FileSave();//生成文本文件
        }
        public void TestMethod(string l_path, string S_path, int num, int outNum)
        {
            path = l_path;   
            number = num;
            outPutNum = outNum;
            s_path = S_path;
            program.FileRead(path);
            program.MainCount();
            program.Output();
            program.FileSave();
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
            }
            catch
            {
                Console.WriteLine("输入存储路径错误！");
            }
        }
        void DataInput()
        {
            Console.WriteLine("输入读取文档路径：");
            path = Console.ReadLine();
            Console.WriteLine("输入想要输出的词组长度：");
            number = int.Parse(Console.ReadLine());
            Console.WriteLine("输出单词数量：");
            outPutNum = int.Parse(Console.ReadLine());
            Console.WriteLine("输出存储路径：");
            s_path = Console.ReadLine();

        }

        void FileRead(string path)
        {        
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Lines.Add(line);
            }
        }
        void MainCount()
        {
            int wordJudge = 0;//单词长度标记
            int MutiWords = 0;//判断目前是否为词组记录状态的标志变量
            int MutiWordsJudge = 0;//词组长度标记
            bool isWord = true;
            string _word = "";
            foreach (var str in Lines)
            {
                int lineJudge = 0;
                foreach (var word in str)
                {
                    //判断当前检测字符是否为空
                    if (word != ' ')
                    {
                        lineJudge++;
                        characters++;
                        //判断当前是否为单词检测状态，判断当前检测字符是否为字母
                        if ((isWord == true) && (((word >= 65) && (word <= 90)) || ((word >= 97) && (word <= 122))))//判断是否为字母内容
                        {
                            wordJudge++;
                            _word = _word + word;
                        }
                        else
                        {
                            //切换至非单词状态
                            if (wordJudge == 0)
                            {
                                isWord = false;
                                MutiWords++;
                            }
                            else
                            {
                                //判断当前是否为单词数字后缀
                                if ((wordJudge >= 4) && ((word >= 48) && (word <= 57)))
                                {
                                    _word = _word + word;
                                }
                                else
                                {
                                    //判断是否已经构成单词
                                    if (wordJudge >= 4)
                                    {
                                        WordAdd(_word);
                                        wordJudge = 0;
                                        _word = "";
                                    }
                                    //结束当前判断周期，重新切换至单词检测状态
                                    else
                                    {
                                        wordJudge = 0;
                                        _word = "";
                                        isWord = true;
                                        MutiWords = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MutiWordsJudge++;
                        //检测到空字符时结束当前判断周期，开启新周期
                        isWord = true;
                        MutiWords = 0;
                        //判断当前检测结果是否构成单词
                        if (wordJudge >= 4)
                        {
                            WordAdd(_word);
                            wordJudge = 0;
                            _word = "";
                        }
                        else
                        {
                            MutiWords++;
                            wordJudge = 0;
                            _word = "";
                            MutiWordsJudge = 0;
                        }
                        if ((MutiWords == 0) && (MutiWordsJudge == number))
                        {
                            MutiWordsAdd();
                            MutiWordsJudge = 0;
                        }
                    }
                }
                //在行末判断是否已构成单词
                if (wordJudge >= 4)
                {
                    WordAdd(_word);
                    wordJudge = 0;
                    _word = "";
                    if ((MutiWords == 0) && (MutiWordsJudge == number - 1))
                    {
                        MutiWordsAdd();
                        MutiWordsJudge = 0;
                    }
                }
                //判断当前行是否为有效行
                if (lineJudge != 0)
                {
                    lines++;
                }
            }
        }
        void WordAdd(string _word)
        {
            int flag = 0;
            foreach (var word in Words)
            {
                if (word.text == _word)
                {
                    word.num++;
                    flag++;
                    break;
                }
            }
            if (flag == 0)
            {
                Word aword = new Word(_word);
                Words.Add(aword);
            }
        }
        void MutiWordsAdd()
        {
            int flag = 0;
            int i = 0;
            int j = Words.Count;
            string text = "";
            for (; i < number; i++)
            {
                text = text + Words[j - number + i].text + " ";
            }
            foreach (var word in MutipleWords)
            {
                if (word.text == text)
                {
                    word.num++;
                    flag++;
                    break;
                }
            }
            if (flag == 0)
            {
                Word mutiWord = new Word(text);
                MutipleWords.Add(mutiWord);
            }
        }
        List<string> WordSort()
        {
            List<Word> HIVword = new List<Word>();
            int i = 0;
            for (; i < Words.Count - 1; i++)
            {
                int j = 0;
                for (; j < Words.Count - 1; j++)
                {
                    if (Words[i].num > Words[j].num)
                    {
                        Word m = Words[i];
                        Words[i] = Words[j];
                        Words[j] = m;
                    }
                }
            }
            i = 0;
            if (Words.Count < outPutNum)
            {
                for (; i < Words.Count; i++)
                {
                    HIVword.Add(Words[i]);
                }
            }
            else
            {
                for (; i < outPutNum; i++)
                {
                    HIVword.Add(Words[i]);
                }
            }

            List<string> words = new List<string>();
            foreach (var word in HIVword)
            {
                words.Add(word.text);
            }
            words.Sort();
            return words;
        }
        void Output()
        {
            List<string> words = program.WordSort();
            Console.WriteLine("字符:" + program.characters);
            SaveStrs.Add("字符:" + program.characters);
            Console.WriteLine("单词:" + program.Words.Count);
            SaveStrs.Add("单词: " + program.Words.Count);
            Console.WriteLine("行:" + program.lines);
            SaveStrs.Add("行:" + program.lines);
            int i = 0;
            //输出高频单词
            if (Words.Count < outPutNum)
            {
                for (; i < Words.Count; i++)
                {
                    foreach (var iword in Words)
                    {
                        if (words[i] == iword.text)
                        {
                            Console.WriteLine(iword.text + " " + iword.num);
                            SaveStrs.Add(iword.text + " " + iword.num);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (; i < outPutNum; i++)
                {
                    foreach (var iword in program.Words)
                    {
                        if (words[i] == iword.text)
                        {
                            Console.WriteLine(iword.text + " " + iword.num);
                            SaveStrs.Add(iword.text + " " + iword.num);
                            break;
                        }
                    }
                }
            }
            //输出词组
            foreach (var word in MutipleWords)
            {
                Console.WriteLine(word.text + " " + word.num);
                SaveStrs.Add(word.text + " " + word.num);
            }
        }
    
        void FileSave()
        {
            System.IO.File.WriteAllLines(s_path, SaveStrs);
        }
    }
   
}