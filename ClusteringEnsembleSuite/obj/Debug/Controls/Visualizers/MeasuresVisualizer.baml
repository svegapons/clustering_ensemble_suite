   M S B A M L   `   `   `  ����R  NClusteringEnsembleSuite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers,ClusteringEnsembleSuite.Controls.Visualizers  5      p lTelerik.Windows.Controls.Navigation, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7pSclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.NavigationTelerik.Windows.Controls 5      e aTelerik.Windows.Controls, Version=2009.2.813.35, Culture=neutral, PublicKeyToken=5803cfa389c90ce7eHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.ControlsTelerik.Windows.Controls 5      J FClusterEnsemble, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullhEclr-namespace:ClusterEnsemble.DataStructures;assembly=ClusterEnsembleClusterEnsemble.DataStructures 5      E    ?ClusteringEnsembleSuite.Controls.Visualizers.MeasuresVisualizer   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      ^this_Controls_Visualizers:clr-namespace:ClusteringEnsembleSuite.Controls.Visualizers    5      `mySclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Navigation    5      [controlsHclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls    5      ace_DataStructuresEclr-namespace:ClusterEnsemble.DataStructures;assembly=ClusterEnsemble    5      -   5          � Title$  Select Measure��5      $
��Auto��6   $	��500��6*    � 
ResizeMode$ NoResize=�66    � Icon$D >/ClusteringEnsembleSuite;component/Resources/Images/config.ico��6e    � 	Resources 5	       �� DataType)  #ClusterEnsemble.DataStructures.Tree �� ItemsSource �� Path��%�   (��       5
   #   *, +)�� 6
   "  6#    6P   �� $ Childs�.R�5      �� ��6   �� $ 
Value.Name�5      5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      �� 5      $
��Auto �6   .+�5   
   *  $Telerik.Windows.Controls.RadTreeView  -   5        $��tv_Measures��5   �   $��2��6   	   IsTriStateMode	 . 
   IsLineEnabled
 .    ItemsOptionListType$ 	CheckList=�6U      IsOptionElementsEnabled . +  %Telerik.Windows.Controls.StyleManager   Theme-  'Telerik.Windows.Controls.ThemeConverter$ Office_Blue 6�   
�������   TVItemStyle#���   6�   65  R   LClusteringEnsembleSuite.Controls.Visualizers.AllParametersMeasuresVisualizer  5   
   -   5          Name$ uctrl_AllParMeasVis��5   T   $��1��6D   $
��Auto��6q   �� 5   
   $��1��6   $��1��6"   $
��Auto��62   
�������.+�5      �� -   5        $��	bt_select��5   (   $�� Select ��6   $��10q�6Q   5      5      5       