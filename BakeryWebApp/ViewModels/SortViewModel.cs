using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public enum SortState
    {
        No,
        DayPlanAcs,
        DayPlanDecs,
        DayProductionAcs,
        DayProductionDecs,
        NormRawNameAcs,
        NormRawNameDecs,
        NormProductNameAcs,
        NormProductNameDecs,
        NormQuantityAcs,
        NormQuantityDecs,
        ProductMaterialMaterialAcs,
        ProductMaterialMaterialDecs,
        ProductMaterialProductAcs,
        ProductMaterialProductDecs,
        ProductMaterialQuantityAcs,
        ProductMaterialQuantityDecs,
        ProductPlanDateAcs,
        ProductPlanDateDecs,
        ProductPlanProductAcs,
        ProductPlanProductDecs,
        ProductPlanCountAcs,
        ProductPlanCountDecs,
        ProductProductionDateAcs,
        ProductProductionDateDecs,
        ProductProductionNameAcs,
        ProductProductionNameDecs,
        ProductProductionCountAcs,
        ProductProductionCountDecs
    }
    public class SortViewModel
    {
        public SortState DayPlanSort { get; set; }
        public SortState DayProductionSort { get; set; }
        public SortState CurrentState { get; set; }
        public SortState NormRawNameSort { get; set; }
        public SortState NormProductNameSort { get; set; }
        public SortState NormQuantitySort { get; set; }
        public SortState ProductMaterialMaterialSort { get; set; }
        public SortState ProductMaterialProductSort { get; set; }
        public SortState ProductMaterialQuantitySort { get; set; }
        public SortState ProductPlanDateSort { get; set; }
        public SortState ProductPlanProductSort { get; set; }
        public SortState ProductPlanCountSort { get; set; }
        public SortState ProductProductionDateSort { get; set; }
        public SortState ProductProductionNameSort { get; set; }
        public SortState ProductProdactionCountSort { get; set; }

        public SortViewModel(SortState sortState)
        {
            DayPlanSort = sortState == SortState.DayPlanAcs ? SortState.DayPlanDecs : SortState.DayPlanAcs;
            DayProductionSort = sortState == SortState.DayProductionAcs ? SortState.DayProductionDecs : SortState.DayProductionAcs;
            NormRawNameSort = sortState == SortState.NormRawNameAcs ? SortState.NormRawNameDecs : SortState.NormRawNameAcs;
            NormProductNameSort = sortState == SortState.NormProductNameAcs ? SortState.NormProductNameDecs : SortState.NormProductNameAcs;
            NormQuantitySort = sortState == SortState.NormQuantityAcs ? SortState.NormQuantityDecs : SortState.NormQuantityAcs;
            ProductMaterialMaterialSort = sortState == SortState.ProductMaterialMaterialAcs ? SortState.ProductMaterialMaterialDecs : SortState.ProductMaterialMaterialAcs;
            ProductMaterialProductSort = sortState == SortState.ProductMaterialProductAcs ? SortState.ProductMaterialProductDecs : SortState.ProductMaterialProductAcs;
            ProductMaterialQuantitySort = sortState == SortState.ProductMaterialQuantityAcs ? SortState.ProductMaterialQuantityDecs : SortState.ProductMaterialQuantityAcs;
            ProductPlanDateSort = sortState == SortState.ProductPlanDateAcs ? SortState.ProductPlanDateDecs : SortState.ProductPlanDateAcs;
            ProductPlanProductSort = sortState == SortState.ProductPlanProductAcs ? SortState.ProductPlanProductDecs : SortState.ProductPlanProductAcs;
            ProductPlanCountSort = sortState == SortState.ProductPlanCountAcs ? SortState.ProductPlanCountDecs : SortState.ProductPlanCountAcs;
            ProductProductionDateSort = sortState == SortState.ProductProductionDateAcs ? SortState.ProductProductionDateDecs : SortState.ProductProductionDateAcs;
            ProductProductionNameSort = sortState == SortState.ProductProductionNameAcs ? SortState.ProductProductionNameDecs : SortState.ProductProductionNameAcs;
            ProductProdactionCountSort = sortState == SortState.ProductProductionCountAcs ? SortState.ProductProductionCountDecs : SortState.ProductProductionCountAcs;
            CurrentState = sortState;
        }
    }
}
