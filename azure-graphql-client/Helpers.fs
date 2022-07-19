namespace DevOpsCentre.Bff

open System
open System.Collections.Generic
open System.Threading.Tasks
open System.IO

module String =
    let join (sep: string) (values: IEnumerable<String>) =
        String.Join(sep, values)

module Stream = 
    let readAsString (stream: Stream) = task {
        use reader = new StreamReader(stream)
        return! reader.ReadToEndAsync()
    }

module Task =
  
  let inline map mapper theTask = task {
    let! res = theTask

    return mapper res
  }
  let inline bind binder theTask = task {
    let! res = theTask

    return! binder res
  }
  let inline lift data = Task.FromResult(data)