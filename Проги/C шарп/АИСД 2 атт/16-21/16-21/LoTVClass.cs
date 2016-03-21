﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace _16_21
{
  struct LoTVElement
  {
    public int element_X;
    public int element_Y;
    public int element_Data;
    public Color element_Color;
  }

  class LoTVNode
  {
    public LoTVNode node_Next;
    public double node_Value;

    public LoTVNode(LoTVNode Next, double Value)
    {
      node_Next = Next;
      node_Value = Value;
    }
  }
  class LoTVStack
  {
    public LoTVNode stack_Head;

    public LoTVStack()
    {
      stack_Head = null;
    }

    public void LoTVStackAdd(double Value)
    {
      LoTVNode NewNode = new LoTVNode(stack_Head, Value);
      stack_Head = NewNode;
    }

    public double LoTVStackTake()
    {
      double Temp = stack_Head.node_Value;
      stack_Head = stack_Head.node_Next;
      return Temp;
    }
  }

  public static class LoTVClass
  {
    static Pen CirclePen = new Pen(Color.Black, 1);
    static LoTVElement[] Elements = new LoTVElement[12];
    static int[] NumsElements = { 92, 22, 65, 66, 46, 25, 81, 37, 26, 33, 91, 66 };
    static LoTVStack Stack_Fibo;
    static LoTVStack Stack_log2;
    static LoTVStack Stack_log3;

    public static void LoTVSetValues()
    {
      Stack_log2 = new LoTVStack();
      Stack_log3 = new LoTVStack();
      Stack_Fibo = new LoTVStack();
      double T_log2 = Math.Truncate(Math.Log(12, 2)) - 1;
      double T_log3 = Math.Truncate(Math.Log(12, 3)) - 1;
      int Length = Elements.Length;
      Stack_log2.LoTVStackAdd(1);
      do
      {
        Stack_log2.LoTVStackAdd(T_log2);
        T_log2 = 2 * T_log2 + 1;
      }
      while (T_log2 < Length);
      do
      {
        Stack_log3.LoTVStackAdd(T_log3);
        T_log3 = 3 * T_log3 + 1;
      }
      while (T_log3 < Length);

      int k = 0;
      int l = 1;
      int temp = 0;
      while (k < Length)
      {
        temp = k;
        k = l;
        l += temp;
        if (k < Length)
          Stack_Fibo.LoTVStackAdd(k);
        else
          break;  //Чтобы не было лишней проверки
      }
    }

    public static void LoTVCreate()
    {
      Random LoTVRnd = new Random();
      for (int i = 0; i < 12; i++)
      {
        Elements[i].element_X = 100;
        Elements[i].element_Y = 20 + i * 45;
        Elements[i].element_Color = Color.Black;
        Elements[i].element_Data = LoTVRnd.Next(0, 100);
        //Elements[i].element_Data = NumsElements[i];
      }
    }

    public static void LoTVSort(int State)
    {
      int j = 0;
      int step = 0;
      switch (State)
      {
        case 1:
          step = Convert.ToInt32(Stack_log2.LoTVStackTake());
          break;
        case 2:
          step = Convert.ToInt32(Stack_log3.LoTVStackTake());
          break;
        case 3:
          step = Convert.ToInt32(Stack_Fibo.LoTVStackTake());
          break;
      }
      while (step > 0)
      {
        for (int i = 0; i < (12 - step); i++)
        {
          j = i;
          int Temp1 = j + step;
          int Temp2 = j;
          bool WasDrawingTouched = false;
          _LoTVMoveTwo(j + step, j, true);
          while ((j >= 0) && (Elements[j].element_Data > Elements[j + step].element_Data))
          {
            if (WasDrawingTouched)
              _LoTVMoveTwo(j + step, j, true);
            int tmp = Elements[j].element_Data;
            Elements[j].element_Data = Elements[j + step].element_Data;
            Elements[j + step].element_Data = tmp;
            LoTV.Draw();
            Thread.Sleep(500);
            j -= step;

            if ((j >= 0) && (Elements[j].element_Data > Elements[j + step].element_Data))
            {
              WasDrawingTouched = true;
              _LoTVMoveTwo(j + step + step, j + step, false);
            }
            else
              if (WasDrawingTouched)
                _LoTVMoveTwo(j + step + step, j + step, false);
              
          }
          if (!WasDrawingTouched)
            _LoTVMoveTwo(Temp1, Temp2, false);
        }

        switch (State)
        {
          case 1:
            step = Stack_log2.stack_Head != null ? Convert.ToInt32(Stack_log2.LoTVStackTake()) : 0;
            break;
          case 2:
            step = Stack_log3.stack_Head != null ? Convert.ToInt32(Stack_log3.LoTVStackTake()) : 0;
            break;
          case 3:
            step = Stack_Fibo.stack_Head != null ? Convert.ToInt32(Stack_Fibo.LoTVStackTake()) : 0;
            break;
        }
      }
    }

    public static void LoTVOutImage(Graphics Canvas)
    {
      Font font = new Font("Courier", 12);
      SizeF size;
      Canvas.Clear(Color.White);
      string Data = null;
      for (int i = 0; i < 12; i++)
      {
        CirclePen.Color = Elements[i].element_Color;
        Canvas.DrawEllipse(CirclePen, Elements[i].element_X, Elements[i].element_Y, 40, 40);
        Data = Elements[i].element_Data.ToString();
        size = Canvas.MeasureString(Data, font);
        Canvas.DrawString(Data, font, Brushes.Black, Elements[i].element_X + 20 - size.Width / 2, Elements[i].element_Y + 20 - size.Height / 2);
      }
    }

    private static void _LoTVMoveTwo(int el1, int el2, bool IsOut)
    {
      if (IsOut)
      {
        Elements[el1].element_Color = Color.Red;
        Elements[el2].element_Color = Color.Red;
        Elements[el1].element_X += 50;
        Elements[el2].element_X += 50;   //Вынос двух элементов вправо
      }
      else
      {
        Elements[el1].element_Color = Color.Black;
        Elements[el2].element_Color = Color.Black;
        Elements[el1].element_X -= 50;
        Elements[el2].element_X -= 50;   //Заносим два элемента влево
      }
      LoTV.Draw();  //Рисуем вынесенные
      Thread.Sleep(500);
    }
  }
}
