import Foundation
import UIKit

// インスタンス化
@_cdecl("init_UISelectionFeedbackGenerator")
func init_UISelectionFeedbackGenerator() -> UnsafeMutableRawPointer {
    let instance : UISelectionFeedbackGenerator
    if #available(iOS 17.5, *),
       let rootView = UnityFramework.getInstance().appController().rootView {
        instance = UISelectionFeedbackGenerator(view: rootView)
    } else {
        instance = UISelectionFeedbackGenerator()
    }
    
    let unmanaged = Unmanaged<UISelectionFeedbackGenerator>.passRetained(instance)
    return unmanaged.toOpaque()
}

// 解放
@_cdecl("release_UISelectionFeedbackGenerator")
func release_UISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr)
    unmanaged.release()
}

// 準備
@_cdecl("prepare_UISelectionFeedbackGenerator")
func prepare_UISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.prepare()
}

// 再生
@_cdecl("selectionChanged_UISelectionFeedbackGenerator")
func selectionChanged_UISelectionFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UISelectionFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.selectionChanged()
}
