//variant 72
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace lab1
{
    internal class Program
		{
			public class Item
			{
				public int weigth;
				public int utility;
				public int position;

				public Item(int w, int u, int p)
				{
					weigth = w;
					utility = u;
					position = p;
				}
		}

public static int Backpack(int bagCapacity, List<Item> items, List<Item> acc)
{
	var itemCount = items.Count;

	int[,] matrix = new int[itemCount + 1, bagCapacity + 1];

	//Go through each item. 
	for (int i = 0; i <= itemCount; i++)
	{
		for (int w = 0; w <= bagCapacity; w++)
		{
			//If we are on the first loop, then set our starting matrix value to 0. 
			if (i == 0 || w == 0)
			{
				matrix[i, w] = 0;
				continue;
			}
			var currentItemIndex = i - 1;
			var currentItem = items[currentItemIndex];
			
			if (currentItem.weigth <= w)
			{
				//Console.WriteLine($"{currentItem.utility + matrix[i - 1, w - currentItem.weigth]} {matrix[i - 1, w]}");
				if (currentItem.utility + matrix[i - 1, w - currentItem.weigth] > matrix[i - 1, w])
				{
					matrix[i, w] = currentItem.utility + matrix[i - 1, w - currentItem.weigth];
				}
				else
				{
					matrix[i, w] = matrix[i - 1, w];
				}
			}
			else
			{
				matrix[i, w] = matrix[i - 1, w];
			}
		}

		// if (taken && prevThrown)
		// {
		// 	if (acc.Count != 0)
		// 	{
		// 		acc.RemoveAt(acc.Count - 1);
		// 		acc.Add(items[i - 1]);
		// 	}
		// }
		// else if (taken && !prevThrown) acc.Add(items[i - 1]);
	}

	return matrix[itemCount, bagCapacity] ;
}

static void Main(string[] args)
{
	int capacity = 0, numTools = 0;
	var items = new List<Item>();
	var accumulator = new List<Item>();
	var fileStream = new FileStream(@"input.txt", FileMode.Open, FileAccess.Read);
	var itemCounter = 1;
	var validParams = true;
	using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
	{
		string initialParams = streamReader.ReadLine();
		try
		{
			numTools = Int32.Parse(initialParams.Split(' ')[0]);
			capacity = Int32.Parse(initialParams.Split(' ')[1]);
		}
		catch (Exception e)
		{
			Console.WriteLine("File must be non-empty!");
			validParams = false;

		}

		string line;
        
		while ((line = streamReader.ReadLine()) != null)
		{
			items.Add(new Item(Int32.Parse(line.Split(' ')[0]), 
				Int32.Parse(line.Split(' ')[1]), itemCounter));
			itemCounter++;
		}
	}

	if (itemCounter != numTools + 1)
	{
		Console.WriteLine("Items count don't match!");
	}
	else if(validParams)
	{
		var selectedItems = Backpack(capacity, items, accumulator);
		// var selectedItemsCount = selectedItems.Count;
		// selectedItems.Sort((a, b) => a.position.CompareTo(b.position));
		// var totalUtility = selectedItems.Sum(x => Convert.ToInt32(x.utility));
		// var usedItemsNum = string.Join(" ", selectedItems.Select(e => e.position));
		Console.WriteLine($"{selectedItems}");
		var writeStream = new FileStream(@"output.txt", FileMode.OpenOrCreate, 
			FileAccess.ReadWrite, FileShare.None);
		using (var streamWriter= new StreamWriter(writeStream, Encoding.UTF8))
		{
			streamWriter.WriteLine(selectedItems);
		}
	}

}
        
    }
}