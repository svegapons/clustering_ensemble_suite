   M S B A M L   `   `   `  ����R  NClusteringEnsembleSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers,ClusteringEnsembleSuite.Controls.Visualizers  5      ?    9ClusteringEnsembleSuite.Controls.TabsControls.TabEnsemble   5      W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ^this_Controls_Visualizers:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers    5      $
��Auto��5      $
��Auto��6   .��5      � .+�5   
   �   � RowDefinitions  5
      �� 5      $
��Auto �6    �� 5       � ColumnDefinitions 5      �� 5      $��3* �6#   �� 5      $
��Auto �6#   �� 5      $��3* �6#   �� 5      $
��Auto �6#   �� 5      $��3* �6#   .+�5      � $��0��6   $��5��6     5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   
���   �$��1q�6=    �� CornerRadius$ 5��6Q   $
��Left=�6b   $��
10 10 10 5q�6}   ._�5      �� 
�������$��1q�64   $��5q�6H    J� BitmapEffect 5      F� 5      ._�5       �� -   5        $��tb_SelectEnsembleAlg��5    $      TblkClicked#���   6@   *(Select Ensemble Clustering Algorithm ...5"      5#      5$      �� 5%      -   5        $��bt_Run��5%      $��1��6'   $
��Left=�67   $��Center=�6R   $��Run Algorithm��6m   $�� 5 0 5 0q�6�   6�   5&      K   EClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer  5(      -   5          Name$. (uctrl_ListClusterAlgVisualizerClustering��5(   A   $��1��6s   $��0��6�   $��5q�6�   �� 5*      -   5        $��gridSplitter1��5*   q   $��1��6   $��1��6)   $��5��69   $��Stretch=�6C    � Cursor$ SizeWE��6a    �� ShowsPreview .  �� ResizeDirection$ Columns=�6�     5,      -   5        $, &uctrl_ListClusterAlgVisualizerEnsemble��5,   A   $��1��6q   $��2��6~   $��5q�6�   �� 5.      -   5        $��gridSplitter2��5.   q   $��1��6   $��3��6)   $��5��69   $��Stretch=�6C   $ SizeWE��6a    . $ Columns=�6�   �� 50      $��1��6   $��4��6$   $��Output��64   $��5q�6D   .��51      �� -   5        $��	tb_output��51   %   $��5q�6    �� 
IsReadOnly . 	 �� TextWrapping$
	 Wrap=�6H   
 �� AcceptsReturn
 .  �� 
AcceptsTab . 6q   52      54      57      58      