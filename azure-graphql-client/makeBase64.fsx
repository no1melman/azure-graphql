open System
open System.Text

let token = "{{ DEVOPS_TOKEN }}"
let user = "{{ DEVOPS_EMAIL }}"

let encode = UTF8Encoding.UTF8.GetBytes($"{user}:{token}")

let base64 = Convert.ToBase64String(encode)

printfn "%s" base64