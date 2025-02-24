import Foundation
import UIKit

// インスタンス化
@_cdecl("init_UIImpactFeedbackGenerator")
func init_UIImpactFeedbackGenerator(_ style: Int32) -> UnsafeMutableRawPointer {
    guard let style = UIImpactFeedbackGenerator.FeedbackStyle(rawValue: Int(style)) else {
        fatalError("invalid type")
    }
    
    let instance: UIImpactFeedbackGenerator
    if #available(iOS 17.5, *),
       let rootView = UnityFramework.getInstance().appController().rootView {
        instance = UIImpactFeedbackGenerator(style: style, view: rootView)
    } else {
        instance = UIImpactFeedbackGenerator(style: style)
    }
    
    let unmanaged = Unmanaged<UIImpactFeedbackGenerator>.passRetained(instance)
    return unmanaged.toOpaque()
}

// 解放
@_cdecl("release_UIImpactFeedbackGenerator")
func release_UIImpactFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<UIImpactFeedbackGenerator>.fromOpaque(instancePtr)
    unmanaged.release()
}

// 準備
@_cdecl("prepare_UIImpactFeedbackGenerator")
func prepare_UIImpactFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UIImpactFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.prepare()
}

// 再生
@_cdecl("impactOccurred_UIImpactFeedbackGenerator")
func impactOccurred_UIImpactFeedbackGenerator(_ instancePtr: UnsafeRawPointer) {
    let instance = Unmanaged<UIImpactFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.impactOccurred()
}

// 再生
// NOTE: intensity の指定は iOS 13 以降より利用可能
@_cdecl("impactOccurred_UIImpactFeedbackGeneratorWithIntensity")
func impactOccurred_UIImpactFeedbackGeneratorWithIntensity(_ instancePtr: UnsafeRawPointer, _ intensity: Float32) {
    let instance = Unmanaged<UIImpactFeedbackGenerator>.fromOpaque(instancePtr).takeUnretainedValue()
    instance.impactOccurred(intensity: CGFloat(intensity))
}
