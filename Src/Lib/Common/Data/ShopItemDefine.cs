using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class ShopItemDefine
    {
        public int ItemID { get; set; }// 道具ID
        public int Count { get; set; }// 数量
        public int Price { get; set; }// 价格
        public int Status { get; set; }// 状态
        public string Remark { get; set; }// 
    }
}
