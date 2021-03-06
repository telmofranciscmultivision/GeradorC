// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace EdpSimulator.Entities
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class JavaCategoryRecord : IEdpRecord
	{
		public static Schema _SCHEMA = Schema.Parse(@"{""type"":""record"",""name"":""Category"",""namespace"":""EdpSimulator.Entities"",""fields"":[{""name"":""transactionid"",""type"":""string""},{""name"":""accountid"",""type"":""string""},{""name"":""category"",""type"":""int""},{""name"":""subcategory"",""type"":""int""},{""name"":""sourceengine"",""type"":""string""},{""name"":""engineversion"",""type"":""string""},{""name"":""modelversion"",""type"":""string""},{""name"":""predictionconfidence"",""type"":""string""}]}");
		private string _transactionid;
		private string _accountid;
		private int _category;
		private int _subcategory;
		private string _sourceengine;
		private string _engineversion;
		private string _modelversion;
		private string _predictionconfidence;
		public virtual Schema Schema
		{
			get
			{
				return JavaCategoryRecord._SCHEMA;
			}
		}
		public string transactionid
		{
			get
			{
				return this._transactionid;
			}
			set
			{
				this._transactionid = value;
			}
		}
		public string accountid
		{
			get
			{
				return this._accountid;
			}
			set
			{
				this._accountid = value;
			}
		}
		public int category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}
		public int subcategory
		{
			get
			{
				return this._subcategory;
			}
			set
			{
				this._subcategory = value;
			}
		}
		public string sourceengine
		{
			get
			{
				return this._sourceengine;
			}
			set
			{
				this._sourceengine = value;
			}
		}
		public string engineversion
		{
			get
			{
				return this._engineversion;
			}
			set
			{
				this._engineversion = value;
			}
		}
		public string modelversion
		{
			get
			{
				return this._modelversion;
			}
			set
			{
				this._modelversion = value;
			}
		}
		public string predictionconfidence
		{
			get
			{
				return this._predictionconfidence;
			}
			set
			{
				this._predictionconfidence = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.transactionid;
			case 1: return this.accountid;
			case 2: return this.category;
			case 3: return this.subcategory;
			case 4: return this.sourceengine;
			case 5: return this.engineversion;
			case 6: return this.modelversion;
			case 7: return this.predictionconfidence;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.transactionid = (System.String)fieldValue; break;
			case 1: this.accountid = (System.String)fieldValue; break;
			case 2: this.category = (System.Int32)fieldValue; break;
			case 3: this.subcategory = (System.Int32)fieldValue; break;
			case 4: this.sourceengine = (System.String)fieldValue; break;
			case 5: this.engineversion = (System.String)fieldValue; break;
			case 6: this.modelversion = (System.String)fieldValue; break;
			case 7: this.predictionconfidence = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}

		public string GetPartitionValue() => this.transactionid;

		public string GetCosmosId() => Guid.NewGuid().ToString();
	}
}
