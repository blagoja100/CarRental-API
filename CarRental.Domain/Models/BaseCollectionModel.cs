using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CarRental.Domain.Models
{
	/// <summary>
	/// Collection model base class.
	/// </summary>
	/// <typeparam name="T">Type of items in the collection.</typeparam>
	public abstract class BaseCollectionModel<T> where T : new()
	{
		/// <summary>
		/// Creates a new collection with initial items.
		/// </summary>
		/// <param name="items"></param>
		protected BaseCollectionModel(IEnumerable<T> items)
		{
			this.SetItems(items);
		}

		/// <summary>
		/// Creates a new collection with initial items.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="startIndex"></param>
		/// <param name="spliceCount"></param>
		protected BaseCollectionModel(IEnumerable<T> items, int startIndex, int spliceCount)
		{
			this.SetItems(items, startIndex, spliceCount);
		}

		/// <summary>
		/// Creates a new empty collection
		/// </summary>
		protected BaseCollectionModel()
		{
		}

		/// <summary>
		/// Helper to set the items and counts in the collection.
		/// </summary>
		/// <param name="items">Items to set.</param>
		public void SetItems(IEnumerable<T> items)
		{
			this.Items = items == null
				? new T[0]
				: items.ToArray();

			this.StartIndex = 0;
			this.Count = this.Items.LongLength;
			this.TotalCount = this.Count;
		}

		/// <summary>
		/// Helper to set the items and counts in the collection.
		/// </summary>
		/// <param name="items">Items to set.</param>
		/// <param name="startIndex"></param>
		/// <param name="spliceCount"></param>
		public void SetItems(IEnumerable<T> items, int startIndex, int spliceCount)
		{
			if (spliceCount > 0)
			{
				items = items.Skip(startIndex * spliceCount).Take(spliceCount);
			}

			this.Items = items == null
				? new T[0]
				: items.ToArray();

			this.StartIndex = startIndex;
			this.Count = this.Items.LongLength;
			this.TotalCount = this.Count;
		}

		/// <summary>
		/// Items in the collection.
		/// </summary>
		public T[] Items { get; private set; } = new T[0];

		/// <summary>
		/// Number of items in this collection.
		/// </summary>
		[Required]
		public long Count { get; set; }

		/// <summary>
		/// Collection start index.
		/// </summary>
		[Required]
		public long StartIndex { get; set; }

		/// <summary>
		/// Total items on the server.
		/// </summary>
		[Required]
		public long TotalCount { get; set; }

		/// <summary>
		/// Adds a new item to the collection and updates counts.
		/// </summary>
		/// <param name="oItem"></param>
		public void Add(T oItem)
		{
			if (oItem == null)
			{
				return;
			}
			var aList = new List<T>(this.Items) { oItem };
			this.Items = aList.ToArray();
			this.Count++;
			this.TotalCount++;
		}

		/// <summary>
		/// Adds items to the collection and updates counts.
		/// </summary>
		/// <param name="aItems"></param>
		public void AddRange(IEnumerable<T> aItems)
		{
			this.Add(aItems);
		}

		/// <summary>
		/// Adds items to the collection and updates counts.
		/// </summary>
		/// <param name="aItems"></param>
		public void Add(IEnumerable<T> aItems)
		{
			if (aItems == null)
			{
				return;
			}
			var aList = new List<T>(this.Items);
			aList.AddRange(aItems);
			this.Items = aList.ToArray();
			var oldCount = this.Count;
			this.Count = this.Items.Length;
			this.TotalCount += this.Count - oldCount;
		}

		/// <summary>
		/// Adds another collection to this collection and updates counts.
		/// </summary>
		/// <param name="collection">Collection to add.</param>
		public void AddRange(BaseCollectionModel<T> collection)
		{
			this.Add(collection);
		}

		/// <summary>
		/// Adds another collection to this collection and updates counts.
		/// </summary>
		/// <param name="collection">Collection to add.</param>
		public void Add(BaseCollectionModel<T> collection)
		{
			if (collection == null)
			{
				return;
			}
			this.Add(collection.Items);
		}
	}
}