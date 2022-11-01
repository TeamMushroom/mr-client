using System;
using System.Collections.Generic;
using UnityEditor;

public static class TpmMenu 
{
    private const string TpmMenuPath = "TPM/";
    
    private const string GenerateCodePath = TpmMenuPath + "Generate Code/";
    
    #region GenerateCode
    
    [MenuItem(GenerateCodePath + "Generate All PatchData IDL", false, 2101)]
    public static void GenerateAllProtobuf()
    {
        var (sheetData, sheetNames) = GameDataCodeGenerator.GetData();
        //ProtobufCodeGenerator.GenerateProtobufEntities((sheetData, sheetNames));
        //ProtobufCodeGenerator.GenerateProtobufEnvelopes((sheetData, sheetNames));
        //ProtobufCodeGenerator.GenerateProtobufEnums();
    }
    
    #endregion
}
