﻿namespace Gjallarhorn.Bindable

open Gjallarhorn
open Gjallarhorn.Validation

open Microsoft.FSharp.Quotations

open System
open System.ComponentModel

/// Interface used to manage a binding target
type IBindingTarget =
    inherit INotifyPropertyChanged
    inherit INotifyDataErrorInfo
    inherit System.IDisposable

    /// Property allowing us to track whether any validation errors currently exist on this target
    abstract member IsValid : bool

    /// Property allowing us to watch our validation state
    abstract member Valid : ISignal<bool>

    /// Adds a disposable to track
    abstract member AddDisposable : System.IDisposable -> unit

    /// Adds a disposable to track from the second element of a tuple, and returns the first element.  Used with Signal subscription functions.
    abstract member AddDisposable2<'a> : ('a * System.IDisposable) -> 'a    

    /// Trigger the PropertyChanged event for a specific property
    abstract RaisePropertyChanged : string -> unit

    /// Trigger the PropertyChanged event for a specific property
    abstract RaisePropertyChanged : Expr -> unit

    /// Track changes on an observable to raise property changed events
    abstract TrackObservable<'a> : string -> IObservable<'a> -> unit

    /// Track changes on an observable of validation results to raise proper validation events, initialized with a starting validation result
    abstract TrackValidator : string -> ValidationResult -> ISignal<ValidationResult> -> unit

    /// Value used to notify signal that an asynchronous operation is executing
    abstract OperationExecuting : bool with get
    
    /// Add a binding target for a signal with a given name, and returns a signal of the user edits
    abstract Bind<'a> : string -> ISignal<'a> -> ISignal<'a>

    /// Add a binding target for a signal for editing with with a given name and validation, and returns a signal of the user edits
    abstract Edit<'a> : string -> (ValidationCollector<'a> -> ValidationCollector<'a>) -> ISignal<'a> -> IValidatedSignal<'a>

    /// Add a readonly binding target for a signal with a given name
    abstract Watch<'a> : string -> ISignal<'a> -> unit

    /// Add a readonly binding target for a constant value with a given name
    abstract Constant<'a> : string -> 'a -> unit

    /// Creates a new command given a binding name
    abstract Command : string -> ITrackingCommand<CommandState>

    /// Creates a new command given a binding name and signal for tracking execution
    abstract CommandChecked : string -> ISignal<bool> -> ITrackingCommand<CommandState>

/// Interface used to manage a typed binding target which outputs changes via IObservable
type IBindingSubject<'b> =
    inherit IBindingTarget
    inherit System.IObservable<'b>

    /// Outputs a value through it's observable implementation
    abstract member OutputValue : 'b -> unit

    /// Outputs values by subscribing to changes on an observable
    abstract member OutputObservable : IObservable<'b> -> unit


