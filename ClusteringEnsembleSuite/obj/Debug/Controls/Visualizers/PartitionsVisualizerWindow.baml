   M S B A M L   `   `   `  ����R  NClusteringEnsembleSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers,ClusteringEnsembleSuite.Controls.Visualizers  5      k gTelerik.Windows.Controls.Input, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7kNclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.InputTelerik.Windows.Controls 5      e aTelerik.Windows.Controls, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7eHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.ControlsTelerik.Windows.Controls 5      M    GClusteringEnsembleSuite.Controls.Visualizers.PartitionsVisualizerWindow   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      PVisualizers:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers    5      ^inputNclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Input    5      `TelerikStylesHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls    5        � Title$   PartitionsVisualizerWindow��5      $	��600��6(   $
��1000��65   
������� � WindowStyle$ 
ToolWindow=�6Y    � Icon$G A/ClusteringEnsembleSuite;component/Resources/Images/linechart.png��6r   .��5      �  � ColumnDefinitions 5	   
   �� 5
      $��2* �6   6,   �� 5      $
��Auto �6   6.   �� 5      $��3* �6   6,    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      6   .+�5   
   �� $��0��6   $��0��6    $��Selected Partition��60   $��5q�6M   .��5      �  5      �� 5      $
��Auto �6'   66   �� 5      $
��Auto �6'   66   �� 5      $
��Auto �6'   66   .+�5      �� $��	Objetive:��6   $��Center=�6-   $��5q�6H   6U   *  $Telerik.Windows.Controls.RadComboBox  5      -   5        $��cb_objetive��5   $   $��1��67   +  %Telerik.Windows.Controls.StyleManager   Theme-  'Telerik.Windows.Controls.ThemeConverter$ Office_Blue 6G   $��5q�6v   $
��Left=�6�   6�   �� 5      -   5        $��2��5      $��	Visualize��6)   $��5q�6=   $�� 5 0 5 0q�6H   6r   5      5      E   ?ClusteringEnsembleSuite.Controls.Visualizers.ListSetsVisualizer  5!   
   -   5          Name$ uctrl_ListSetsVisualizer��5!   )   $��1��6L   $��0��6Y   $��5q�6i   �� 5#   
   -   5        $��gridSplitter1��5#   �   $��0��6   $��2��6%   $��1��66   $��3��6F   $��0 15 0 5q�6P   
�������$��Stretch=�6y    � Cursor$ SizeWE��6�    �� ShowsPreview . 	 �� ResizeDirection$	 Columns=�6�   �� 5%   
   $��2��6   $��2��6$   $��2D Visualize��64   $��5q�6K   .��5&      F   @ClusteringEnsembleSuite.Controls.Visualizers.PartitionVisualizer  -   5        
  Name$
 ctrl_partvisualize��5&   9   $��5q�6.   6W   5'      5(      5)      