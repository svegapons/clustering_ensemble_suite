   M S B A M L   `   `   `  ����R  NClusteringEnsembleSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers,ClusteringEnsembleSuite.Controls.Visualizers  5      k gTelerik.Windows.Controls.Input, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7kNclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.InputTelerik.Windows.Controls 5      e aTelerik.Windows.Controls, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7eHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.ControlsTelerik.Windows.Controls 5      A    ;ClusteringEnsembleSuite.Controls.TabsControls.TabEvaluation   5      W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ^this_Controls_Visualizers:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers    5      [myNclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Input    5      `TelerikStylesHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls    5      $
��Auto��5      $
��Auto��6     � 	Resources  5       �� 
TargetType   	pathStyle �� Property �� Value �� 
StartPoint �� EndPoint � Color � Offset  HeaderTemplateArrowUp F�Name  HeaderTemplateArrowDown��%�  &	        &	 7    7   &	     7   �� 5	   
   " F�6   .�5
      ��  � �� �	#ff21448C6,   �� 5       � �� 5      �� 5      $	 0,1&�6*   $	 0,0&�6;   .��5      � $ 	#ff21448C��6'   $ 0Q�69   � 5      $ 	#ffE7E2FD��6'   $ 1Q�69   5      5      �� 5       � ��$ 1��65   5      �� 5   
   .R�5      ]� .+�5      �� $��Center=�6   ��69   �� F� 5      $ arrow��6   $��1��5      8��  +����@KL ��� ���� ��� р�� @KL �@KL ��� 	5      5      5      �� 5   
   .R�5      ]� .+�5      �� $��Center=�6   ��69   �� F� 5       $ arrow��6   $��1��5!      8��  +����@KL @KL р�� ��� ���� @KL �@KL @KL 	5"      5#      5$      5%      .��5&      � .+�5(   
   � 	 � RowDefinitions	 5*      �� 5+      $
��Auto �6    60   �� 5,      6#   
 � ColumnDefinitions
 5.      �� 5/      $��3* �6#   60   �� 50      $
��Auto �6#   62   �� 51      $��3* �6#   60   �� 52      $
��Auto �6#   62   �� 53      $��3* �6#   60   .+�56      � $��0��6   $��5��6    
 57      �� 58      $
��Auto �6'   66   �� 59      $
��Auto �6'   66   �� 5:      $
��Auto �6'   66   .+�5<      �� 
���   �$��0��6-   $��1q�6=    �� CornerRadius$ 5��6Q   $
��Left=�6b   $��
10 10 10 5q�6}   ._�5=      �� 
�������$��1q�64   $��5q�6H    J� BitmapEffect 5>      F� 5?      ._�5A      �� -   5        $��tb_SelectMeasure��5A   $     TblkClicked#���  6<   Select Measures ...5C      5D      5E      �� 5G      -   5        $��bt_Run��5G      $��1��6'   $
��Left=�67   $��Center=�6R   $��Evaluate Structurings��6m   $�� 5 0 5 0q�6�   6�   �� 5H      -   5        $��bt_Save��5H      $��2��6(   $��Center=�68   $��Center=�6U   $��Save Evaluation��6p   $�� 5 0 5 0q�6�   $��10q�6�   6�   5J      K   EClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer  5L      -   5          Name$1 +uctrl_ListClusterAlgVisualizerClustEnsemble��5L   A   $��1��6v   $��0��6�   $��5q�6�   �� 5N      -   5        $��gridSplitter1��5N   q   $��1��6   $��1��6)   $��5��69   $��Stretch=�6C    � Cursor$ SizeWE��6a    �� ShowsPreview .  �� ResizeDirection$ Columns=�6�   �� 5P      $��1��6   $��2��6$   $��Measures��64   $��5q�6F   .��5Q      � 	 5R      �� 5S      $
��Auto �6(   68   �� 5T      6+   
 5W      �� 5X      $
��Auto �6+   6:   �� 5Y      6-   �� 5Z      $
��Auto �6+   6:   .+�5]      �� -   5        $��chb_selectAll��5]      $��5 0 5 5q�64   $��Center=�6E   $��
Select all��6`   6�   �� 5_      $��1��6    $��5 0 0 5q�60   $��Center=�6A   $��Right=�6\   $$��Objective(Externals Measures):��6x   6�   *  $Telerik.Windows.Controls.RadComboBox  5a      -   5        $��cb_objetive��5a   %   $��2��68   +  %Telerik.Windows.Controls.StyleManager   Theme-  'Telerik.Windows.Controls.ThemeConverter$ Office_Blue 6H   $��5q�6w   6�   �� 5c      -   5        $��lv_Measures��5c      $��1��62   $��3��6?    �� SelectionMode$ Multiple=�6S     LVItemStyle#���  6l   $��5 0 5 5q�6�    �� View 5d      �� 5e      .P�5g   "   �� $��State��62    �� CellTemplate 5h   &   �� 5i   *   .R�5j   .   ��  d� 	IsChecked 67   ��  �� Path$ 
IsSelected� �� RelativeSource �� *FindAncestor+ �� AncestorType" ��5l   +   5n   #   �� 5p   "   $��Measure��61    5q   &   �� 5r   *   .R�5s   .   �� ��68   �� *MeasureName+5t   +   5v   #   5x      5{      5|      5}      �� 5      -	   5        $��gridSplitter2��5   q   $��1��6   $��3��6)   $��5��69   $��Stretch=�6C   $ SizeWE��6a    . $ Columns=�6�   �� 5�      $��1��6   $��4��6$   $��Output��64   $��5q�6D   .��5�      �� -
   5        $��lv_MeasuresOutput��5�      #���  64   $��5q�6�    5�      �� 5�      6%   5�      5�      5�      5�      5�      