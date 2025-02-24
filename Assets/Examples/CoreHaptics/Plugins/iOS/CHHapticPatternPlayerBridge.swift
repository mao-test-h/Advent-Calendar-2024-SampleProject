import Foundation
import CoreHaptics

@_cdecl("release_CHHapticPatternPlayer")
func release_CHHapticPatternPlayer(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<CHHapticPatternPlayer>.fromOpaque(instancePtr)
    unmanaged.release()
}

@_cdecl("start_CHHapticPatternPlayer")
func start_CHHapticPatternPlayer(
    _ instancePtr: UnsafeRawPointer,
    _ atTime: Float32,
    _ onError: OnErrorDelegate)
{
    let player = Unmanaged<CHHapticPatternPlayer>.fromOpaque(instancePtr).takeUnretainedValue()
    do {
        let atTime = atTime == 0 ? CHHapticTimeImmediate : TimeInterval(atTime)
        try player.start(atTime: atTime)
    } catch let error {
        onError(error.localizedDescription.toCharPtr())
    }
}

@_cdecl("stop_CHHapticPatternPlayer")
func stop_CHHapticPatternPlayer(
    _ instancePtr: UnsafeRawPointer,
    _ atTime: Float32,
    _ onError: OnErrorDelegate)
{
    let player = Unmanaged<CHHapticPatternPlayer>.fromOpaque(instancePtr).takeUnretainedValue()
    do {
        let atTime = atTime == 0 ? CHHapticTimeImmediate : TimeInterval(atTime)
        try player.stop(atTime: atTime)
    } catch let error {
        onError(error.localizedDescription.toCharPtr())
    }
}

@_cdecl("cancel_CHHapticPatternPlayer")
func cancel_CHHapticPatternPlayer(
    _ instancePtr: UnsafeRawPointer,
    _ onError: OnErrorDelegate)
{
    let player = Unmanaged<CHHapticPatternPlayer>.fromOpaque(instancePtr).takeUnretainedValue()
    do {
        try player.cancel()
    } catch let error {
        onError(error.localizedDescription.toCharPtr())
    }
}

@_cdecl("sendParameters_CHHapticPatternPlayer")
func sendParameters_CHHapticPatternPlayer(
    _ instancePtr: UnsafeRawPointer,
    _ parametersPtr: UnsafeRawPointer,
    _ parametersLength: Int32,
    _ atTime: Float32,
    _ onError: OnErrorDelegate)
{
    let player = Unmanaged<CHHapticPatternPlayer>.fromOpaque(instancePtr).takeUnretainedValue()
    
    // UnsafeRawPointer から [BlittableDynamicParameter] への変換
    let count = Int(parametersLength)
    let typedPointer = parametersPtr.bindMemory(to: BlittableDynamicParameter.self, capacity: count)
    let buffer = UnsafeBufferPointer(start: typedPointer, count: count)
    let blittableDynamicParams = Array(buffer)
    
    // [BlittableDynamicParameter] を [CHHapticDynamicParameter] に変換
    var dynamicParams = [CHHapticDynamicParameter]()
    for param in blittableDynamicParams {
        let dynamicParam = CHHapticDynamicParameter(
            parameterID: dynamicParameterForInt(Int(param.parameterId)),
            value: Float(param.parameterValue),
            relativeTime: TimeInterval(param.time))
        dynamicParams.append(dynamicParam)
    }
    
    do {
        let atTime = atTime == 0 ? CHHapticTimeImmediate : TimeInterval(atTime)
        try player.sendParameters(dynamicParams, atTime: atTime)
    }catch let error {
        
        onError(error.localizedDescription.toCharPtr())
    }
}

struct BlittableDynamicParameter {
    let parameterId: Int32
    let time: Float32
    let parameterValue: Float32
}

// refered to: https://github.com/apple/unityplugins/blob/main/plug-ins/Apple.CoreHaptics/Native/CoreHapticsWrapper/Haptics/Utilities.swift
@available(iOS 13, tvOS 14, macOS 10, *)
func dynamicParameterForInt(_ val: Int) -> CHHapticDynamicParameter.ID {
    switch val {
    case 0:
        return CHHapticDynamicParameter.ID.hapticIntensityControl
    case 1:
        return CHHapticDynamicParameter.ID.hapticSharpnessControl
    case 2:
        return CHHapticDynamicParameter.ID.hapticAttackTimeControl
    case 3:
        return CHHapticDynamicParameter.ID.hapticDecayTimeControl
    case 4:
        return CHHapticDynamicParameter.ID.hapticReleaseTimeControl
    case 5:
        return CHHapticDynamicParameter.ID.audioVolumeControl
    case 6:
        return CHHapticDynamicParameter.ID.audioPanControl
    case 7:
        return CHHapticDynamicParameter.ID.audioPitchControl
    case 8:
        return CHHapticDynamicParameter.ID.audioBrightnessControl
    case 9:
        return CHHapticDynamicParameter.ID.audioAttackTimeControl
    case 10:
        return CHHapticDynamicParameter.ID.audioDecayTimeControl
    case 11:
        return CHHapticDynamicParameter.ID.audioReleaseTimeControl
    default:
        return CHHapticDynamicParameter.ID.hapticIntensityControl
    }
}
