using JetBrains.ReSharper.Daemon.CodeFolding;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Folding
{
    public class SpecFlowFoldingProcessor : TreeNodeVisitor<FoldingHighlightingConsumer>, ICodeFoldingProcessor
    {

        public bool InteriorShouldBeProcessed(ITreeNode element, FoldingHighlightingConsumer context) => true;

        public bool IsProcessingFinished(FoldingHighlightingConsumer context) => false;

        public void ProcessBeforeInterior(ITreeNode node, FoldingHighlightingConsumer consumer)
        {
            switch (node)
            {
                case GherkinScenario scenario:
                    FoldNode(node, consumer, scenario.IsBackground() ? GherkinTokenTypes.BACKGROUND_KEYWORD : GherkinTokenTypes.SCENARIO_KEYWORD);
                    break;
                case GherkinScenarioOutline scenarioOutline:
                    FoldNode(node, consumer, scenarioOutline.IsBackground() ? GherkinTokenTypes.BACKGROUND_KEYWORD :GherkinTokenTypes.SCENARIO_OUTLINE_KEYWORD);
                    break;
                case GherkinExamplesBlock _:
                    FoldNode(node, consumer, GherkinTokenTypes.EXAMPLES_KEYWORD);
                    break;
                case GherkinRule _:
                    FoldNode(node, consumer, GherkinTokenTypes.RULE_KEYWORD);
                    break;
            }

        }

        private static void FoldNode(ITreeNode node, FoldingHighlightingConsumer consumer, GherkinTokenType keyWordTokenType)
        {
            var gherkinElement = node as GherkinElement;
            var keyword = gherkinElement.FindChild<GherkinToken>(o => o.NodeType == keyWordTokenType)?.GetText();
            var text  = gherkinElement.FindChild<GherkinToken>(o => o.NodeType == GherkinTokenTypes.TEXT)?.GetText();

            consumer.AddTreeNodeFolding(CodeFoldingAttributes.DEFAULT_FOLDING_ATTRIBUTE, node, $"{keyword}: {text}");
        }

        public void ProcessAfterInterior(ITreeNode element, FoldingHighlightingConsumer consumer)
        {
            
        }
        
    }
}