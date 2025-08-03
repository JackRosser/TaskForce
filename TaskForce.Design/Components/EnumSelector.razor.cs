using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace TaskForce.Design.Components;

/// <summary>
/// Componente generico per la selezione di un enum. Supporta anche gli enum nullable con TEnum?.
/// </summary>
/// <typeparam name="TEnum">Tipo di Enum</typeparam>
public partial class EnumSelector<TEnum>
{
    [Parameter, EditorRequired] public TEnum? Value { get; set; }
    [Parameter] public required Expression<Func<TEnum?>> ValueExpression { get; set; }
    [Parameter] public EventCallback<TEnum?> ValueChanged { get; set; }

    /// <summary>
    /// Id del campo </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Label da mostrare sopra la combobox.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Imposta se mostrare l'opzione di default che equivale a non aver fatto alcuna scelta.
    /// </summary>
    [Parameter]
    public bool ShowDefaultOption { get; set; } = false;

    /// <summary>
    /// Testo per l'opzione di default se diverso da quello predefinito: "Seleziona un valore...".'
    /// </summary>
    [Parameter]
    public string? DefaultOptionText { get; set; }

    // ... (rest of the code unchanged)

    private IEnumerable<TEnum> EnumValues => System.Enum.GetValues(UnderlyingType).Cast<TEnum>();

    private static Type UnderlyingType => Nullable.GetUnderlyingType(typeof(TEnum)) ?? typeof(TEnum);

}