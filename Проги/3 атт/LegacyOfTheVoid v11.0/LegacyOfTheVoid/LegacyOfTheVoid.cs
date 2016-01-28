﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ProjectRed
{
  public partial class LegacyOfTheVoid : Form
  {
    FolderBrowserDialog fbd = new FolderBrowserDialog();
    OpenFileDialog ofd = new OpenFileDialog();
    Journal Jour;
    int tempLesson = 0;  //переменная для отсроченного обновления в эвенте gridLessonNames
    int tempClass = 0; //Переменная для отсроч эвента gridClasses
    bool tempClassActive = true; //Индексатор активного класса
    bool tempFirstOn = true; //Индексатор первого запуска

    enum NumIDs
    {
      Лекция,       //0
      Практика,     //1
      Семинар,      //2
      Аттестация,   //3
      Экзамен,      //4
      Зачет         //5
    }   //==================================================================ИД типов занятий

    public LegacyOfTheVoid() //==========================================================Форма
    {
      InitializeComponent();
      ofd.DefaultExt = "*.txt";
      ofd.Filter = "TXT files|*.txt";
    }

    //--------------------------------------------------------------------------------Кнопки

    private void loadJournal_Click(object sender, EventArgs e) //========================Загрузка журнала из выбранного файла 100%
    {
      if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        try
        {
          ResetData();
          GetJournalToProgram();        
          DrawClassListOnGrid();
          chbStudList.Checked = true;
          chbLessonList.Checked = true;
        }
        catch
        {
          MessageBox.Show("Невозможно открыть выбранный файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ResetData() //==========================================================Сброс таблиц и листов 100%
    {
      Jour = new Journal(null, null, null);
      gridClasses.ColumnCount = 0;
      gridLessonNames.ColumnCount = 0;
      gridLessonTexts.ColumnCount = 0;
      gridMiddleMarks.ColumnCount = 0;
      gridStudentMarks.ColumnCount = 0;
      gridStudentNames.ColumnCount = 0;
    }

    private void saveJournal_Click(object sender, EventArgs e) //========================Сохранение журнала в файл 100%
    {
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.DefaultExt = "*.txt";
      sfd.Filter = "TXT files|*.txt";
      if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        GetSetChecks(gridClasses.CurrentCellAddress.Y, gridLessonNames.CurrentCellAddress.Y, true);
        MarksMatrix(gridClasses.CurrentCellAddress.Y, gridLessonNames.CurrentCellAddress.Y, false);
        File.WriteAllText(sfd.FileName, Jour.Save());
      }
    }

    private void menuLoadStudList_Click(object sender, EventArgs e) //===================Загрузка файла дебилов 100% 
    {
      if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        try
        {
          GetStudsFileToProgram();
          DrawClassListOnGrid();
          chbStudList.Checked = true;
        }
        catch
        {
          MessageBox.Show("Невозможно открыть выбранный файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void menuLoadLessonList_Click(object sender, EventArgs e) //=================Загрузка файла плана 100% 
    {
      if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        try
        {
          GetLesssFileToProgram();
          chbLessonList.Checked = true;
        }
        catch
        {
          MessageBox.Show("Невозможно открыть выбранный файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    //--------------------------------------------------------------------------------Получатели файлов в распоряжение программы

    private void GetStudsFileToProgram() //==============================================Получение файла студентов 100%
    {
      StreamReader Txt = new StreamReader(ofd.FileName);
      Jour.Groups = Journal.GetGroupList(Txt.ReadToEnd());
      Txt.Close();
      if (chbLessonList.Checked == true)
        MatrixPreLoader();
    }

    private void GetLesssFileToProgram() //==============================================Получение учебного плана 100%
    {
      StreamReader Txt = new StreamReader(ofd.FileName);
      Jour.Lessons = Journal.GetLessonsList(Txt.ReadToEnd());
      Txt.Close();
      if (chbStudList.Checked == true)
        MatrixPreLoader();
    }

    private void GetJournalToProgram()   //==============================================Получение и парсинг файла журнала 100%
    {
      StreamReader Txt = new StreamReader(ofd.FileName);
      Jour = Journal.Parse(Txt.ReadToEnd());  
      
      chbLessonList.Checked = true;
      chbStudList.Checked = true;
      Txt.Close();
    }

    //--------------------------------------------------------------------------------Спецфункции таблиц оценок

    private void MatrixPreLoader()  //===================================================Предзапись матриц оценок 100%
    {
      for (int i = 0; i < Jour.Groups.Count; i++)
      {
        List<LessonMarks> GroupMarks = new List<LessonMarks>();
        for (int j = 0; j < Jour.Lessons[i].Subjects.Count; j++)
        {
          string[,] Matrix = new string[Jour.Groups[i].StudNames.Length, Jour.Lessons[i].Subjects[j].Dates.Count];
          string[] MiddleMarks = new string[Jour.Groups[i].StudNames.Length];
          bool[] IsWere = new bool[Jour.Lessons[i].Subjects[j].Dates.Count];
          GroupMarks.Add(new LessonMarks(Jour.Lessons[i].Subjects[j].LessonName, Jour.Lessons[i].Subjects[j].Dates, Matrix, MiddleMarks, IsWere));
        }
        Jour.StudMarks.Add(new MarksForClass(Jour.Groups[i].GroupName, GroupMarks));
      }
    }

    private void MatrixUpdater(int ClassID, int LessonID) //=============================Обновление матрицы класса ClassID для урока LessonID в листе WholeMarks 100%
    {
      for (int i = 0; i < gridStudentMarks.RowCount; i++)
      {
        for (int j = 0; j < gridStudentMarks.ColumnCount; j++)
          Jour.StudMarks[ClassID].Marks[LessonID].MarksMatrix[i, j] = Convert.ToString(gridStudentMarks.Rows[i].Cells[j].Value);
        Jour.StudMarks[ClassID].Marks[LessonID].MiddleMarks[i] = Convert.ToString(gridMiddleMarks.Rows[i].Cells[0].Value);
      }
    }

    private void ReturnMatrixToGrid(int ClassID, int LessonID) //========================Возврат матрицы на форму 100%
    {
      for (int i = 0; i < gridStudentMarks.RowCount; i++)
      {
        for (int j = 0; j < gridStudentMarks.ColumnCount; j++)
          gridStudentMarks.Rows[i].Cells[j].Value = Jour.StudMarks[ClassID].Marks[LessonID].MarksMatrix[i, j];
        gridMiddleMarks.Rows[i].Cells[0].Value = Jour.StudMarks[ClassID].Marks[LessonID].MiddleMarks[i];
      }
    }

    private void MarksMatrix(int ClassID, int LessonID, bool Return) //==================Запись-выдача матриц оценок в зависимости от Return 100%
    {
      for (int i = 0; i < gridStudentMarks.RowCount; i++)
      {
        for (int j = 0; j < gridStudentMarks.ColumnCount; j++)
          if (Return)
          {
            gridStudentMarks.Rows[i].Cells[j].Value = Jour.StudMarks[ClassID].Marks[LessonID].MarksMatrix[i, j];
            gridMiddleMarks.Rows[i].Cells[0].Value = Jour.StudMarks[ClassID].Marks[LessonID].MiddleMarks[i];
          }
          else
            if (!tempClassActive)
          {
            Jour.StudMarks[ClassID].Marks[LessonID].MarksMatrix[i, j] = Convert.ToString(gridStudentMarks.Rows[i].Cells[j].Value);
            Jour.StudMarks[ClassID].Marks[LessonID].MiddleMarks[i] = Convert.ToString(gridMiddleMarks.Rows[i].Cells[0].Value);
          }
      }
    }

    private void GetSetChecks(int ClassID, int LessonID, bool IsGet)  //================================Сбор данных чекбоксов 100%
    {
      for (int i = 0; i < Jour.Lessons[ClassID].Subjects[LessonID].Themes.Count; i++)
        if (IsGet)
          Jour.StudMarks[ClassID].Marks[LessonID].IsWas[i] = Convert.ToBoolean(gridLessonTexts.Rows[i].Cells[3].Value);
        else
          gridLessonTexts.Rows[i].Cells[3].Value = Jour.StudMarks[ClassID].Marks[LessonID].IsWas[i];
    }

    //--------------------------------------------------------------------------------Рисовалки

    private void DrawClassListOnGrid() //================================================Разрисовка списка классов 100%
    {
      gridClasses.RowCount = Jour.Groups.Count;
      for (int i = 0; i < Jour.Groups.Count; i++)
        gridClasses.Rows[i].Cells[0].Value = Jour.Groups[i].GroupName;
    }

    private void DrawNamesListOnGrid(int ClassNum) //====================================Разрисовка списка имен 100%
    {
      gridStudentNames.RowCount = Jour.Groups[ClassNum].StudNames.Length;
      for (int i = 0; i < Jour.Groups[ClassNum].StudNames.Length; i++)
        gridStudentNames.Rows[i].Cells[0].Value = Jour.Groups[ClassNum].StudNames[i];
      gridStudentNames.Columns[0].HeaderText = "Дата";
      gridStudentNames.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
    }

    private void DrawLessonsOfSelectedClassOnGrid(int Num) //============================Разрисовка учебного плана для выбранного класса 100%
    {
      gridLessonNames.RowCount = Jour.Lessons[Num].Subjects.Count;
      for (int i = 0; i < Jour.Lessons[Num].Subjects.Count; i++)
        gridLessonNames.Rows[i].Cells[0].Value = Jour.Lessons[Num].Subjects[i].LessonName;         
        gridLessonNames.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;      
    }

    private void DrawLessonsThemesAndDates(int LessonNum, int ClassNum) //===============Разрисовка тем, дат и типов занятий 100%
    {
      gridLessonTexts.RowCount = Jour.Lessons[ClassNum].Subjects[LessonNum].Themes.Count;
      gridLessonTexts.ColumnCount = 3;
      gridLessonTexts.Columns.Insert(3, new DataGridViewCheckBoxColumn(false));

      for (int i = 0; i < Jour.Lessons[ClassNum].Subjects[LessonNum].Themes.Count; i++)
      {
        gridLessonTexts.Rows[i].Cells[0].Value = Jour.Lessons[ClassNum].Subjects[LessonNum].Dates[i];
        gridLessonTexts.Rows[i].Cells[1].Value = Jour.Lessons[ClassNum].Subjects[LessonNum].Themes[i];
        gridLessonTexts.Rows[i].Cells[2].Value = (NumIDs)Jour.Lessons[ClassNum].Subjects[LessonNum].Types[i];
        gridLessonTexts.Rows[i].Cells[3].Value = false;
      }

      gridLessonTexts.Columns[0].Width = 35;
      gridLessonTexts.Columns[1].Width = 120;
      gridLessonTexts.Columns[2].Width = 65;
      gridLessonTexts.Columns[3].Width = 47;

      gridLessonTexts.Columns[0].HeaderText = "Дата";
      gridLessonTexts.Columns[1].HeaderText = "Тема";
      gridLessonTexts.Columns[2].HeaderText = "Тип";
      gridLessonTexts.Columns[3].HeaderText = "Статус";

      for (int i = 0; i < 3; i++)
        gridLessonTexts.Columns[i].ReadOnly = true;
      foreach (DataGridViewColumn column in gridLessonTexts.Columns)
        column.SortMode = DataGridViewColumnSortMode.NotSortable;  
    }

    private void DrawMarksGrid(int LessonNum, int ClassNum) //Разрисовка таблицы оценок 100%
    {
      gridStudentMarks.RowCount = gridStudentNames.RowCount;
      gridStudentMarks.ColumnCount = Jour.Lessons[ClassNum].Subjects[LessonNum].Dates.Count;

      gridMiddleMarks.RowCount = gridStudentNames.RowCount;

      for (int i = 0; i < gridStudentMarks.ColumnCount; i++)
        gridStudentMarks.Columns[i].Width = 40;
      for (int i = 0; i < Jour.Lessons[ClassNum].Subjects[LessonNum].Themes.Count; i++)
        gridStudentMarks.Columns[i].HeaderText = Jour.Lessons[ClassNum].Subjects[LessonNum].Dates[i];

      gridMiddleMarks.Columns[0].Width = 67;
      gridMiddleMarks.Columns[0].HeaderText = "Среднее";
        gridMiddleMarks.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
        foreach (DataGridViewColumn column in gridStudentMarks.Columns)
          column.SortMode = DataGridViewColumnSortMode.NotSortable; 
    }

    private void MiddleFinder(int RowNumber)  //=========================================Разрисовка таблицы средних значений 100%
    {
      List<string> Mass = new List<string>();
      for (int i = 0; i < gridStudentMarks.ColumnCount; i++)
        if (gridStudentMarks.Rows[RowNumber].Cells[i].Value != null)
        Mass.Add(Convert.ToString(gridStudentMarks.Rows[RowNumber].Cells[i].Value));
      if (Mass.Count != 0)
      gridMiddleMarks.Rows[RowNumber].Cells[0].Value = MiddleLogics.Middle(Mass);
    }

    //--------------------------------------------------------------------------------Обработчики событий 

    private void gridClasses_CellClick(object sender, DataGridViewCellEventArgs e) //====Обработчик события нажатия клетки в gridClasses 100%
    {
      if (chbLessonList.Checked == true)
      {
        tempFirstOn = false;
        if (gridLessonNames.RowCount == 0 || gridLessonTexts.RowCount == 0)
        {
          tempClass = e.RowIndex;
          DrawNamesListOnGrid(e.RowIndex);
          DrawLessonsOfSelectedClassOnGrid(e.RowIndex);
        }
        else
        {
          if (gridLessonTexts.RowCount != 0)
          {
            GetSetChecks(tempClass, tempLesson, true);
            MarksMatrix(tempClass, tempLesson, false);
          }
            tempClass = e.RowIndex;
            DrawNamesListOnGrid(e.RowIndex);
            DrawLessonsOfSelectedClassOnGrid(e.RowIndex);
            gridStudentMarks.Columns.Clear();
            gridLessonTexts.Columns.Clear();
            gridMiddleMarks.Columns.Clear();          
        }
        tempClassActive = true;
      }
      else
        MessageBox.Show("Не загружен учебный план.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void gridLessonNames_CellClick(object sender, DataGridViewCellEventArgs e) //Обработчик события нажатия ячейки в таблице предметов 100%
    {
      if (tempClassActive)
      {
        DrawLessonsThemesAndDates(e.RowIndex, gridClasses.CurrentCellAddress.Y);
        tempClassActive = false;
        DrawMarksGrid(e.RowIndex, gridClasses.CurrentCellAddress.Y);
        tempLesson = e.RowIndex;
        if (!tempFirstOn)
        {
          GetSetChecks(gridClasses.CurrentCellAddress.Y, tempLesson, false);
          MarksMatrix(gridClasses.CurrentCellAddress.Y, tempLesson, true);
        }
      }
      else
      {
        GetSetChecks(gridClasses.CurrentCellAddress.Y, tempLesson, true);
        MarksMatrix(gridClasses.CurrentCellAddress.Y, tempLesson, false);
        DrawLessonsThemesAndDates(e.RowIndex, gridClasses.CurrentCellAddress.Y);
        tempLesson = e.RowIndex;
        gridStudentMarks.Columns.Clear();
        DrawMarksGrid(tempLesson, gridClasses.CurrentCellAddress.Y);
        GetSetChecks(gridClasses.CurrentCellAddress.Y, tempLesson, false);
        MarksMatrix(gridClasses.CurrentCellAddress.Y, tempLesson, true);
      }
    }

    private void gridStudentMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)//Обработчик события переключения на другую ячейку в табле оценок 100%
    {
      MiddleFinder(e.RowIndex);
    }

    //--------------------------------------------------------------------------------Весь мусор тут

    private void menuHelp_Click(object sender, EventArgs e) //===========================Помощь 100%
    {
      MessageBox.Show("Чтобы начать работу с программой, откройте меню Файл и добавьте списки учеников и учебные планы.", "Помощь");
    }

    private void menuClose_Click(object sender, EventArgs e) //==========================Закрыть форму 100%
    {
      this.Close();
    }

    private void helpAbout_Click(object sender, EventArgs e) //==========================О программе 100%
    {
      MessageBox.Show("Разработчик данного школьного журнала - студент 1 курса ФКН, ВГУ Леонтьев Максим (aka Mexahoid).", "О программе");
    }
  }
}