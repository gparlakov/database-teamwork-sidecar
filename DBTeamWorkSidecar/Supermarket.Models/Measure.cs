//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Supermarket.Models
//{
//    public class Measure_
//    {
//        private int _iD;
//        public virtual int ID
//        {
//            get
//            {
//                return this._iD;
//            }
//            set
//            {
//                this._iD = value;
//            }
//        }

//        private string _measureName;
//        public virtual string MeasureName
//        {
//            get
//            {
//                return this._measureName;
//            }
//            set
//            {
//                this._measureName = value;
//            }
//        }

//        private IList<Product> _products = new List<Product>();//makes connection one to many
//        public virtual IList<Product> Products
//        {
//            get
//            {
//                return this._products;
//            }
//        }
//    }
//}
