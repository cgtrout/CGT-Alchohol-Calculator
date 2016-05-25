namespace BloodAlcoholCalculator.ViewModel
{
     //a class implementing this interface will convert from modelType to VmType
     public interface ViewModelConvertor<VmType, ModelType>
     {
          VmType ConvertFrom(ModelType type);
          ModelType GetBaseType();

          //get id of underlying modeltype
          long GetBaseId();
     }
}