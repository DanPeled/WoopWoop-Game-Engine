// using System.Diagnostics;
// using CompileTimeWeaver;
// namespace WoopWoopEngine
// {

//     public class TestAttribute : AdviceAttribute
//     {
//         public override object Advise(IInvocation invocation)
//         {
//             Console.WriteLine("Entering " + invocation.Method.Name);
//             try
//             {
//                 return invocation.Proceed();
//             }
//             catch (Exception e)
//             {
//                 Trace.WriteLine("MyAdvice catches an exception: " + e.Message);
//                 throw;
//             }
//         }
//         public override async Task<object> AdviseAsync(IInvocation invocation)
//         {
//             Trace.WriteLine("Entering async " + invocation.Method.Name);

//             try
//             {
//                 return await invocation.ProceedAsync().ConfigureAwait(false);   // asynchroniously call the next advice in advice pipeline, or call target method
//             }
//             catch (Exception e)
//             {
//                 Trace.WriteLine("MyAdvice catches an exception: " + e.Message);
//                 throw;
//             }
//             finally
//             {
//                 Trace.WriteLine("Leaving async " + invocation.Method.Name);
//             }
//         }
//     }
// } // TODO: create a custom argument that will cache out values of functions in order to save computing time