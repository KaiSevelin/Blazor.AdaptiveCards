using System;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards.Blazor.Actions;
using Microsoft.AspNetCore.Components;

namespace AdaptiveCards.Blazor
{
    public abstract class AdaptiveCardsBase<TModel> : ComponentBase
    {
        [Parameter]
        public string Schema { get; set; }

        [Parameter]
        public RenderFragment<TModel> RowTemplate { get; set; }

        [Parameter]
        public List<TModel> Models { get; set; }

        [Parameter]
        public Func<TModel, string> TemplateSelector { get; set; }

        [Parameter]
        public string @class { get; set; } = "row";

        [Parameter]
        public string CardClass { get; set; } = "col-3";

        [Parameter]
        public Dictionary<string, object> CardAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public List<(string, Func<int, TModel, object>)> CardAttributeFunctions { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        [Parameter]
        public RenderMode RenderMode { get; set; } = RenderMode.Synchronous;

        /// <summary>
        /// Gets or sets the handler which is executed when opening link.
        /// </summary>
        /// <value>The handler.</value>
        [Parameter] public EventCallback<string> OnOpenLinkAction { get; set; }

        /// <summary>
        /// Gets or sets the submit action handler.
        /// </summary>
        /// <value>The handler.</value>
        [Parameter] public EventCallback<SubmitEventArgs> OnSubmitAction { get; set; }

        /// <summary>
        /// Gets or sets the callback when a card is rendered.
        /// </summary>
        /// <value>The on card rendered.</value>
        [Parameter] public EventCallback<CardRenderedEventArgs> OnCardRendered { get; set; }

        /// <summary>
        /// Gets or sets the callback when rendering the card fails.
        /// </summary>
        /// <value>The on card render failed.</value>
        [Parameter] public EventCallback<CardRenderFailedEventArgs> OnCardRenderFailed { get; set; }

        /// <summary>
        /// Gets or sets the submit handler.
        /// </summary>
        /// <value>The submit handler.</value>
        [Parameter] public object SubmitHandler { get; set; }

        protected Dictionary<string, object> GetCardAttributes(int index, TModel model)
        {
            var result = new Dictionary<string, object>(CardAttributes);

            if (!string.IsNullOrWhiteSpace(CardClass) && !result.ContainsKey("class"))
            {
                result.Add("class", CardClass);
            }

            if (CardAttributeFunctions?.Any() != true)
            {
                return result;
            }

            foreach (var cardAttribute in CardAttributeFunctions)
            {
                result.Add(cardAttribute.Item1, cardAttribute.Item2(index, model));
            }

            return result;
        }
    }
}
