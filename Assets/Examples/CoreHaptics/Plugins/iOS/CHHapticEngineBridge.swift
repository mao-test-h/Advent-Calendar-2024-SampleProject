import Foundation
import CoreHaptics

@_cdecl("supportsHaptics_CHHapticEngine")
func supportsHaptics_CHHapticEngine() -> UInt8 {
    return CHHapticEngine.capabilitiesForHardware().supportsHaptics ? 1 : 0
}

@_cdecl("init_CHHapticEngine")
func init_CHHapticEngine(
    _ onCreateError: OnErrorDelegate,
    _ onStopped: OnStoppedDelegate,
    _ onReset: OnResetDelegate)
-> UnsafeMutableRawPointer? {
    do {
        let engine = try CHHapticEngine()
        
        // NOTE: コールバックは基本的に MainThread に戻してから返すようにする
        engine.stoppedHandler = { reason in
            DispatchQueue.main.async {
                onStopped(Int32(reason.rawValue))
            }
        }
        
        engine.resetHandler = {
            DispatchQueue.main.async {
                onReset()
            }
        }
        
        let unmanaged = Unmanaged<CHHapticEngine>.passRetained(engine)
        return unmanaged.toOpaque()
    } catch let error {
        onCreateError(error.localizedDescription.toCharPtr())
        return nil
    }
}

@_cdecl("release_CHHapticEngine")
func release_CHHapticEngine(_ instancePtr: UnsafeRawPointer) {
    let unmanaged = Unmanaged<CHHapticEngine>.fromOpaque(instancePtr)
    unmanaged.release()
}

@_cdecl("start_CHHapticEngine")
func start_CHHapticEngine(_ instancePtr: UnsafeRawPointer, _ onError: OnErrorDelegate) {
    let engine = Unmanaged<CHHapticEngine>.fromOpaque(instancePtr).takeUnretainedValue()
    engine.start { error in
        if let error = error {
            DispatchQueue.main.async {
                onError(error.localizedDescription.toCharPtr())
            }
            return
        }
    }
}

@_cdecl("stop_CHHapticEngine")
func stop_CHHapticEngine(_ instancePtr: UnsafeRawPointer, _ onError: OnErrorDelegate) {
    let engine = Unmanaged<CHHapticEngine>.fromOpaque(instancePtr).takeUnretainedValue()
    engine.stop { error in
        if let error = error {
            DispatchQueue.main.async {
                onError(error.localizedDescription.toCharPtr())
            }
            return
        }
    }
}

@_cdecl("makePlayer_CHHapticEngine")
func makePlayer_CHHapticEngine(
    _ instancePtr: UnsafeRawPointer,
    _ ahapStrPtr: CCharPtr,
    _ onError: OnErrorDelegate)
-> UnsafeMutableRawPointer? {
    let engine = Unmanaged<CHHapticEngine>.fromOpaque(instancePtr).takeUnretainedValue()
    let ahapStr = ahapStrPtr.toString()
    do {
        let dict = try JSONSerialization.jsonObject(with: Data(ahapStr.utf8), options: [])
        guard let patternDict = dict as? [CHHapticPattern.Key: Any] else {
            onError("Failed JSONSerialization".toCharPtr())
            return nil
        }
        
        let hapticPattern = try CHHapticPattern(dictionary: patternDict)
        let player = try engine.makePlayer(with: hapticPattern)
        let unmanaged = Unmanaged<CHHapticPatternPlayer>.passRetained(player)
        return unmanaged.toOpaque()
    } catch let error {
        onError(error.localizedDescription.toCharPtr())
        return nil
    }
}

@_cdecl("playPattern_CHHapticEngine")
func playPattern_CHHapticEngine(
    _ instancePtr: UnsafeRawPointer,
    _ ahapStrPtr: CCharPtr,
    _ onError: OnErrorDelegate)
{
    let engine = Unmanaged<CHHapticEngine>.fromOpaque(instancePtr).takeUnretainedValue()
    let ahapStr = ahapStrPtr.toString()
    do {
        if let data = ahapStr.data(using: .utf8) {
            try engine.playPattern(from: data)
        }
    } catch let error {
        onError(error.localizedDescription.toCharPtr())
    }
}
