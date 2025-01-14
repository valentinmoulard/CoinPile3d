// using System.Reflection;
//
// public static class ConsoleUtilities 
// {
//     public static void ClearConsole()
//     {
//         var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
//         var type = assembly.GetType("UnityEditor.LogEntries");
//         var method = type.GetMethod("Clear");
//         method.Invoke(new object(), null);
//     }
// }
