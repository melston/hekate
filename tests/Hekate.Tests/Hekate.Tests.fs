﻿module Hekate.Tests

open Hekate
open Swensen.Unquote
open Xunit

(* Fixtures *)

let private g1 =
    Graph.empty

let private g2 =
    Graph.create 
        [ 1, "one"
          2, "two"
          3, "three" ]
        [ 2, 1, "left"
          3, 1, "up"
          1, 2, "right"
          2, 3, "down" ]

(* Construction *)

[<Fact>]
let ``addNode behaves correctly`` () =
    let g3 = Graph.Nodes.add (4, "four") g2

    Graph.Nodes.count g3 =! 4
    Graph.Nodes.count g3 =! 4

[<Fact>]
let ``removeNode behaves correctly`` () =
    let g3 = Graph.Nodes.remove 1 g2

    Graph.Nodes.count g3 =! 2
    Graph.Edges.count g3 =! 1

[<Fact>]
let ``addEdge behaves correctly`` () =
    let g3 = Graph.Edges.add (1, 3, "down") g2

    Graph.Nodes.count g3 =! 3
    Graph.Edges.count g3 =! 5

[<Fact>]
let ``removeEdge behaves correctly`` () =
    let g3 = Graph.Edges.remove (2, 1) g2

    Graph.Nodes.count g3 =! 3
    Graph.Edges.count g3 =! 3

(* Queries *)

[<Fact>]
let ``containsEdge behaves correctly`` () =
    Graph.Edges.contains 1 2 g2 =! true
    Graph.Edges.contains 1 3 g2 =! false

[<Fact>]
let ``containsNode behaves correctly`` () =
    Graph.Nodes.contains 1 g2 =! true
    Graph.Nodes.contains 4 g2 =! false

[<Fact>]
let ``isEmpty behaves correctly`` () =
    Graph.isEmpty g1 =! true
    Graph.isEmpty g2 =! false

(* Mapping *)

[<Fact>]
let ``mapEdges behaves correctly`` () =
    let g3 = Graph.Edges.map (fun v1 v2 (e: string) -> sprintf "%i.%i.%s" v1 v2 e) g2

    Graph.Edges.find 1 2 g3 =! (1, 2, "1.2.right")

[<Fact>]
let ``mapNodes behaves correctly`` () =
    let g3 = Graph.Nodes.map (fun _ (n: string) -> n.ToUpper ()) g2

    snd (Graph.Nodes.find 1 g2) =! "one"
    snd (Graph.Nodes.find 1 g3) =! "ONE"

(* Projection *)

[<Fact>]
let ``nodes behaves correctly`` () =
    List.length (Graph.Nodes.toList g2) =! 3

[<Fact>]
let ``edges behaves correctly`` () =
    List.length (Graph.Edges.toList g2) =! 4

(* Inspection *)

[<Fact>]
let ``tryFindNode behaves correctly`` () =
    Graph.Nodes.tryFind 1 g2 =! Some (1, "one")
    Graph.Nodes.tryFind 4 g2 =! None

[<Fact>]
let ``findNode behaves correctly`` () =
    Graph.Nodes.find 1 g2 =! (1, "one")
    raises<exn> <@ Graph.Nodes.find 4 g2 @>

[<Fact>]
let ``rev behaves correctly`` () =
    let g3 = Graph.rev g2
    let g4 = Graph.Edges.remove (1, 3) g3

    Graph.Edges.count g3 =! 4
    Graph.Edges.count g4 =! 3

(* Adjacency/Degree *)

[<Fact>]
let ``neighbours behaves correctly`` () =
    Graph.Nodes.neighbours 1 g2
        =! Some [ 2, "left"
                  3, "up"
                  2, "right" ]

[<Fact>]
let ``successors behaves correctly`` () =
    Graph.Nodes.successors 1 g2
        =! Some [ 2, "right" ]

[<Fact>]
let ``predecessors behaves correctly`` () =
    Graph.Nodes.predecessors 1 g2 
        =! Some [ 2, "left"
                  3, "up" ]

[<Fact>]
let ``outward behaves correctly`` () =
    Graph.Nodes.outward 1 g2 
        =! Some [ 1, 2, "right" ]

[<Fact>]
let ``inward behaves correctly`` () =
    Graph.Nodes.inward 1 g2 
        =! Some [ 2, 1, "left"
                  3, 1, "up" ]

[<Fact>]
let ``degree behaves correctly`` () =
    Graph.Nodes.degree 1 g2 
        =! Some 3

[<Fact>]
let ``outwardDegree behaves correctly`` () =
    Graph.Nodes.outwardDegree 1 g2 
        =! Some 1

[<Fact>]
let ``inwardDegree behaves correctly`` () =
    Graph.Nodes.inwardDegree 1 g2 
        =! Some 2