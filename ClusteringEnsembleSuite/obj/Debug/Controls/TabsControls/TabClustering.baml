   M S B A M L   `   `   `  ����R  NClusteringEnsembleSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers,ClusteringEnsembleSuite.Controls.Visualizers  5      A    ;ClusteringEnsembleSuite.Controls.TabsControls.TabClustering   5      W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ^this_Controls_Visualizers:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers    5      $
��Auto��5      $
��Auto��6   .��5      � .+�5   
   �   � RowDefinitions  5	      �� 5
      $
��Auto �6    �� 5       � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $
��Auto �6#   �� 5      $��2* �6#   .+�5      � $��0��6   $��3��6     5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5      �� $��0��6   
���   �$��1q�6=    �� CornerRadius$ 5��6Q   $
��Left=�6b   $��
10 10 10 5q�6}   ._�5      �� 
�������$��1q�64   $��5q�6H    J� BitmapEffect 5      F� 5      ._�5      �� -   5        $��tb_SelectClusterAlg��5   $      TblkClicked#���   6?   !Select Clustering Algorithm ...5!      5"      5#      �� 5%      -   5        $��bt_Run��5%      $��1��6'   $
��Left=�67   $��Center=�6R   $��Run Algorithm��6m   $�� 5 0 5 0q�6�   6�   �� 5'      -   5        $��gb_Exp��5'      $��2��6)   $��Right=�69   $��Center=�6U   $��Experimentos��6p   $��20 1 1 1q�6�   $�� 5 0 5 0q�6�   .��5(      �� -   5        $��sp_Exp��5(   !    �� Orientation$ 
Horizontal=�60   .+�5)      �� -   5        $��chbx_AttrRnd��5)   #   $��10q�67   $��Random Attributes��6C   �� 5*      -   5        $��	bt_RunAll��5*   !   $��Run All��6J   $��10q�6\   $��3q�6h   �� 5+      -   5        $��tb_Count��5+   "   $��10��62   $��10q�6<   5,      5-      5.      K   EClusteringEnsembleSuite.Controls.Visualizers.ListClusterAlgVIsualizer  50      -   5          Name$$ uctrl_ListClusterAlgVisualizer��50   A   $��1��6i   $��0��6v   $��5q�6�   �� 52      -	   5        $��gridSplitter1��52   q   $��1��6   $��1��6)   $��5��69   $��Stretch=�6C    � Cursor$ SizeWE��6a    �� ShowsPreview .  �� ResizeDirection$ Columns=�6�   �� 54      $��1��6   $��2��6$   $��Output��64   $��5q�6D   .��55      �� -
   5        $��	tb_output��55      $��5q�6+   	 �� 
IsReadOnly	 . 
 �� TextWrapping$

 Wrap=�6H    �� AcceptsReturn .  �� 
AcceptsTab . 6q   56      59      5=      5>      