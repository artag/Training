﻿using ApplicationServices.Interfaces;
using CsvHelper.Configuration;

namespace ApplicationServices
{
    public sealed class ProductFileRecordMap : ClassMap<ProductRecordDto>
    {
        public ProductFileRecordMap()
        {
            AutoMap();
            Map(m => m.UnitPrice).Name("Unit Price").ConvertUsing(c => (c.UnitPrice ?? 0).ToString("C"));
        }
    }
}
